using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : NetworkBehaviour
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
        public float currentAC = 100f;

        [SerializeField] private float maxAC = 100f;
        [SerializeField] private float useMultiplier = 10f;
        [SerializeField] private float reloadMultiplier = 150f;
        private bool disabled = false;
        private float disablingTime;
        private float disabledFor;

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

        public bool Disabled
        {
            get
            {
                if (disabled)
                    if (Time.time > disablingTime + disabledFor) //Si il a été désactivé pour le temps voulu, il se réactive
                        disabled = false;
                return disabled;
            }
        }

        public void Use()
        {
            //currentAC -= Time.deltaTime * useMultiplier;
            //if (currentAC < 0)
            //    currentAC = 0;
        }

        public void Reload()
        {
            currentAC += Time.deltaTime * reloadMultiplier;
            if (currentAC > maxAC) currentAC = maxAC;
        }

        public void Enable()
        {
            disabledFor = 0;
            disabled = false;
        }

        public void Disable(float time = 0)
        {
            disabled = true;
            disablingTime = Time.time;
            disabledFor = time;
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

    //public RectTransform _UIAirControl;//A ENLEVER (CACA) ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■ 8===D

    private Weight playerWeight;
    [SerializeField, Range(0f, 1f)] private float decelerationPercentage = 0.1f;

    private UIManager UIManager;

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        playerWeight = GetComponent<Weight>();
    }

    private void OnEnable()
    {
        mouseLook.Init(transform, cam.transform);
        //UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }


    private void Update()
    {
        if (!isLocalPlayer)
            return;

        RotateView();

        //A ENLEVER !!!!!!!
        //_UIAirControl.localScale = new Vector3(_UIAirControl.localScale.x, airControl.currentAC/100, _UIAirControl.localScale.z);

        if (Input.GetButtonDown("Jump") && !m_Jump)
        {
            m_Jump = true;
        }

    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        Vector2 input = GetInput();

        movementSettings.UpdateDesiredTargetSpeed(input);

        if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon))
        {
            if (((m_IsGrounded || m_IsRoofed) || airControl.Useable) && !airControl.Disabled)
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
                   // GetComponent<SoundPlayerManager>().StartDeplacement();
                    //Debug.Log("Start");
                    m_RigidBody.AddForce(desiredMove, ForceMode.Impulse);
                }
            }
        }
        else
        {
            //GetComponent<SoundPlayerManager>().StopDeplacement();
            //Debug.Log("Stop");
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

        if (!airControl.Disabled)
        {
            // Rotate the rigidbody velocity to match the new direction that the character is looking
            Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
            m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
        }
    }

    private void ApplyDeceleration()
    {
        if (!airControl.Disabled)
        {
            if ((m_IsGrounded || m_IsRoofed || m_Jumpable) && !m_Jumping)
                m_RigidBody.velocity *= decelerationPercentage;
            else
            {
                float yAxisVelocity = m_RigidBody.velocity.y;
                m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x * decelerationPercentage, yAxisVelocity, m_RigidBody.velocity.z * decelerationPercentage);
            }
        }
    }

    public void DisableAirControl(float time)
    {
        if (playerWeight.CurrentWeight == 0)
            time = Mathf.Infinity;
        airControl.Disable(time);
    }

    [ClientRpc]
    public void RpcDisableAirControl(float time)
    {
        DisableAirControl(time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isLocalPlayer)
            return;
        airControl.Enable();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isLocalPlayer)
            return;

        string objectTag = collision.collider.tag;
        if (collision.collider.tag == "Ground" || collision.collider.tag == "Roof" || collision.collider.tag == "Jumpable")
        {
            m_PreviouslyOnPlane = (m_IsGrounded || m_IsRoofed || m_Jumpable);

            Vector3 normal = collision.contacts[0].normal;
            bool notWallRun = Mathf.Abs(normal.y) > 0.5 ? true : false;
            if (notWallRun)
            {
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
                        m_Jumpable = true;
                        break;
                }

                if (!m_PreviouslyOnPlane && (m_IsGrounded || m_IsRoofed || m_Jumpable) && m_Jumping)
                    m_Jumping = false;
            }
            else
            {
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
        if (!isLocalPlayer)
            return;

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

}
