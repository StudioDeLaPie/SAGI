using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    [Serializable]
    public class MovementSettings
    {
        public float ForwardSpeed = 65;   // Speed when walking forward
        public float StrafeSpeed = 45;    // Speed when walking sideways or backwards
        public float JumpForce = 150;
        [HideInInspector] public float CurrentTargetSpeed;

        public void UpdateDesiredTargetSpeed(Vector2 input)
        {
            if (input == Vector2.zero) return;
            if (input.x > 0 || input.x < 0) //strafe
            {
                CurrentTargetSpeed = StrafeSpeed;
            }
            if (input.y < 0) //backwards
            {
                CurrentTargetSpeed = StrafeSpeed;
            }
            if (input.y > 0) //forwards
            {
                //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                CurrentTargetSpeed = ForwardSpeed;
            }
        }
    }

    [Serializable]
    public class AirControl
    {
        public float currentAC;

        [SerializeField] private float maxAC = 100f;
        [SerializeField] private float useMultiplier = 10f;
        [SerializeField] private float reloadMultiplier = 150f;

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
            currentAC += Time.deltaTime * reloadMultiplier;
            if (currentAC > maxAC) currentAC = maxAC;
        }
    }

    public Camera cam;
    public MovementSettings movementSettings = new MovementSettings();
    public MouseLook mouseLook = new MouseLook();
    public AirControl airControl = new AirControl();

    private Rigidbody m_RigidBody;
    private Vector3 m_GroundContactNormal;
    private bool m_Jump, m_Jumping, m_IsGrounded, m_IsRoofed, m_Jumpable;
    private bool m_PreviouslyOnPlane; //Stocke si on était précédemment sur un sol, le plafond ou un objet duquel on peut sauter

    private Weight playerWeight;
    [SerializeField, Range(0f, 1f)] private float decelerationPercentage = 0.1f;


    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
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
        Debug.Log("Grounded : " + m_IsGrounded + "   Roofed : " + m_IsRoofed + "    Jumpable : " + m_Jumpable + "  Air control : " + airControl.currentAC);
    }

    private void FixedUpdate()
    {
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
                    m_RigidBody.AddForce(desiredMove, ForceMode.Impulse);
                }
            }
        }

        if (m_IsGrounded || m_IsRoofed)
        {
            airControl.Reload(); //Rechargement du air control
        }

        if ((m_IsGrounded || m_IsRoofed || m_Jumpable) && m_Jump)
        {
            m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);

            if (m_IsGrounded || (m_Jumpable && playerWeight.CurrentWeight >= 0))
                m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
            else if (m_IsRoofed || (m_Jumpable && playerWeight.CurrentWeight <= 0))
                m_RigidBody.AddForce(new Vector3(0f, -movementSettings.JumpForce, 0f), ForceMode.Impulse);
            m_Jumping = true;
        }
        m_Jump = false;
        ApplyDeceleration(); //Application de la resistance au sol et a l'air
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

    private void ApplyDeceleration()
    {
        if ((m_IsGrounded || m_IsRoofed || m_Jumpable) && !m_Jumping)
            m_RigidBody.velocity *= decelerationPercentage;
        else
        {
            float yAxisVelocity = m_RigidBody.velocity.y;
            m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x * decelerationPercentage, yAxisVelocity, m_RigidBody.velocity.z * decelerationPercentage);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        string objectTag = collision.collider.tag;
        if (collision.collider.tag == "Ground" || collision.collider.tag == "Roof" || collision.collider.tag == "Jumpable")
        {
            m_PreviouslyOnPlane = (m_IsGrounded || m_IsRoofed || m_Jumpable);

            Vector3 normal = collision.contacts[0].normal;
            Debug.DrawRay(collision.contacts[0].point, normal, Color.red, 2);
            bool notWallRun = Mathf.Abs(normal.y) > 0.5 ? true : false;
            if (notWallRun)
            {
                Debug.DrawRay(collision.contacts[0].point, normal, Color.green, 2);
                m_GroundContactNormal = normal; //La normale sur laquelle on se déplace

                switch (objectTag)
                {
                    case "Ground":
                        m_IsGrounded = true;
                        break;

                    case "Roof":
                        m_IsRoofed = true;
                        break;

                    case "Jumpable":
                        //if ((playerWeight.CurrentWeight >= 0 && Vector3.Angle(Vector3.up, normal) < 40f) || (playerWeight.CurrentWeight <= 0 && Vector3.Angle(Vector3.down, normal) < 40f))
                        //{
                            //Debug.Log("On peut sauter  " + Vector3.Angle(Vector3.up, normal));
                            m_Jumpable = true;
                        //}
                        break;
                }

                if (!m_PreviouslyOnPlane && (m_IsGrounded || m_IsRoofed || m_Jumpable) && m_Jumping)
                    m_Jumping = false;
            }
            else
            {
                Debug.DrawRay(collision.contacts[0].point, normal, Color.red, 2);
                switch (objectTag)
                {
                    case "Ground":
                        m_IsGrounded = false;
                        break;

                    case "Roof":
                        m_IsRoofed = false;
                        break;

                    case "Jumpable":
                        m_Jumpable = false;
                        break;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        m_PreviouslyOnPlane = (m_IsGrounded || m_IsRoofed || m_Jumpable);

        if (collision.collider.tag == "Ground" || collision.collider.tag == "Roof" || collision.collider.tag == "Jumpable")
        {
            m_GroundContactNormal = Vector3.up;
            switch (collision.collider.tag)
            {
                case "Ground":
                    m_IsGrounded = false;
                    break;
                case "Roof":
                    m_IsRoofed = false;
                    break;
                case "Jumpable":
                    m_Jumpable = false;
                    break;

            }
        }
    }

    /// <summary>
    /// Demande au controleur de s'appliquer une force d'ejection dans la direction donnee
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
