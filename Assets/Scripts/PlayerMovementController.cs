using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovementController : MonoBehaviour
{
    [Serializable]
    public class MovementSettings
    {
        public float ForwardSpeed = 65;   // Speed when walking forward
        public float StrafeSpeed = 45;    // Speed when walking sideways or backwards
        public float JumpForce = 150;
        public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
        [HideInInspector] public float CurrentTargetSpeed;

        public void UpdateDesiredTargetSpeed(Vector2 input)
        {
            if (input == Vector2.zero) return;
            if (input.x > 0 || input.x < 0)
            {
                //strafe
                CurrentTargetSpeed = StrafeSpeed;
            }
            if (input.y < 0)
            {
                //backwards
                CurrentTargetSpeed = StrafeSpeed;
            }
            if (input.y > 0)
            {
                //forwards
                //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                CurrentTargetSpeed = ForwardSpeed;
            }
        }
    }


    [Serializable]
    public class AdvancedSettings
    {
        public float groundRoofCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
        [Tooltip("set it to 0.1 or more if you get stuck in wall")]
        public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
    }

    [Serializable]
    public class AirControl
    {
        public float currentAC;

        [SerializeField] private float maxAC = 100f;
        [SerializeField] private float useMultiplier = 1f;
        [SerializeField] private float reloadMultiplier = 20f;

        public AirControl()
        {
            currentAC = maxAC;
        }

        public bool Useable
        {
            get
            {
                return currentAC > 0;
            }
        }

        public void Use()
        {
            currentAC -= Time.deltaTime * useMultiplier;
            if (currentAC < 0)
                currentAC = 0;
        }

        public void Reload()
        {
            currentAC += Time.deltaTime * 20;
            if (currentAC > maxAC) currentAC = maxAC;
        }
    }

    public Camera cam;
    public MovementSettings movementSettings = new MovementSettings();
    public MouseLook mouseLook = new MouseLook();
    public AdvancedSettings advancedSettings = new AdvancedSettings();
    public AirControl airControl = new AirControl();

    private Rigidbody m_RigidBody;
    private CapsuleCollider m_Capsule;
    private float m_YRotation;
    private Vector3 m_GroundContactNormal;
    private bool m_Jump, m_PreviouslyGrounded, m_PreviouslyRoofed, m_Jumping, m_IsGrounded, m_IsRoofed;

    private PlayerWeight playerWeight;
    [SerializeField, Range(0f, 1f)] private float decelerationPercentage = 0.1f;


    public Vector3 Velocity
    {
        get { return m_RigidBody.velocity; }
    }

    public bool Grounded
    {
        get { return m_IsGrounded; }
    }

    public bool Jumping
    {
        get { return m_Jumping; }
    }

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        playerWeight = GetComponent<Weight>();
        mouseLook.Init(transform, cam.transform);
    }


    private void Update()
    {
        RotateView();

        if (Input.GetButtonDown("Jump") && !m_Jump)
        {
            m_Jump = true;
        }
        Debug.Log(m_IsGrounded + "    roof : " + m_IsRoofed + "    Air control : " + airControl.currentAC);
    }

    private void FixedUpdate()
    {
        GroundAndRoofCheck();

        Vector2 input = GetInput();

        movementSettings.UpdateDesiredTargetSpeed(input);

        if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon))
        {
            if ((m_IsGrounded || m_IsRoofed) || airControl.Useable)
            {
                if (!m_IsGrounded && !m_IsRoofed)
                    airControl.Use(); //Utilisation du air control

                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;
                desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

                desiredMove.x = desiredMove.x * movementSettings.CurrentTargetSpeed;
                desiredMove.y = desiredMove.y * movementSettings.CurrentTargetSpeed;
                desiredMove.z = desiredMove.z * movementSettings.CurrentTargetSpeed;
                if (m_RigidBody.velocity.sqrMagnitude <
                    (movementSettings.CurrentTargetSpeed * movementSettings.CurrentTargetSpeed))
                {
                    m_RigidBody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
                    //m_RigidBody.AddForce(desiredMove, ForceMode.Impulse);
                }
            }
        }

        if (m_IsGrounded || m_IsRoofed)
        {
            airControl.Reload(); //Rechargement du air control
            if (m_Jump)
            {
                m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
                if (m_IsGrounded)
                    m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
                else
                    m_RigidBody.AddForce(new Vector3(0f, -movementSettings.JumpForce, 0f), ForceMode.Impulse);
                m_Jumping = true;
            }
        }
        m_Jump = false;
        ApplyDeceleration(); //Application de la r�sistance au sol et � l'air
    }


    private float SlopeMultiplier()
    {
        float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
        if (m_IsRoofed)
            angle = Vector3.Angle(m_GroundContactNormal, Vector3.down);
        return movementSettings.SlopeCurveModifier.Evaluate(angle);
    }

    private Vector2 GetInput()
    {
        Vector2 input = new Vector2
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        };
        return input;
    }

    private void RotateView()
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = transform.eulerAngles.y;

        mouseLook.LookRotation(transform, cam.transform);

        // Rotate the rigidbody velocity to match the new direction that the character is looking
        Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
        m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
    }

    /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
    private void GroundAndRoofCheck()
    {
        m_PreviouslyGrounded = m_IsGrounded;
        m_PreviouslyRoofed = m_IsRoofed;

        RaycastHit hitInfo;

        #region GROUND
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                               ((m_Capsule.height / 2f) - m_Capsule.radius) + advancedSettings.groundRoofCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            m_IsGrounded = true;
            m_GroundContactNormal = hitInfo.normal;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundContactNormal = Vector3.up;
        }
        if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
        {
            m_Jumping = false;
        }
        #endregion
        hitInfo = new RaycastHit();
        #region ROOF
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.up, out hitInfo,
                               ((m_Capsule.height / 2f) - m_Capsule.radius) + advancedSettings.groundRoofCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            m_IsRoofed = true;
            m_GroundContactNormal = hitInfo.normal;
        }
        else
        {
            m_IsRoofed = false;
            m_GroundContactNormal = Vector3.up;
        }
        if (!m_PreviouslyRoofed && m_IsRoofed && m_Jumping)
        {
            m_Jumping = false;
        }
        #endregion
    }

    private void ApplyDeceleration()
    {
        if (m_IsGrounded || m_IsRoofed)
            m_RigidBody.velocity *= decelerationPercentage;
        else
        {
            float y = m_RigidBody.velocity.y;
            m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x * decelerationPercentage, y, m_RigidBody.velocity.z * decelerationPercentage);
        }
    }

    /// <summary>
    /// Demande au contr�leur de s'appliquer une force d'�jection dans la direction donn�e
    /// </summary>
    //[ClientRpc]
    //public void RpcRepulsion(Vector3 playerPosition, Vector3 hitPosition)
    //{
    //    Vector3 repulsion = Vector3.ProjectOnPlane((hitPosition - playerPosition), Vector3.up).normalized; //direction
    //    repulsion *= movementSettings.JumpForce; //force horizontale
    //    repulsion += Vector3.up * movementSettings.JumpForce; //force verticale

    //    advancedSettings.airControl = false; //d�sactivation du airControl jusqu'� collision avec autre chose
    //    m_RigidBody.velocity = Vector3.zero;
    //    m_RigidBody.AddForce(repulsion, ForceMode.Impulse); //Application de la force

    //}
}
