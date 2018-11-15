using UnityEngine;
using UnityEngine.UI;

public class PlayerWeight : MonoBehaviour
{
    private float maxWeight = 500;
    private float minGravityWeight = 50;
    public int weightChange = 5;

    //ConnectionPlayer connectionPlayer;
    [SerializeField] private float weight; //syncvar
    [SerializeField] private Text txtWeight;
    private Rigidbody rb;

    public float Weight
    {
        get
        {
            return weight;
        }
    }

    public float MaxWeight
    {
        get
        {
            return maxWeight;
        }
    }

    void Awake()
    {
        //connectionPlayer = GetComponent<ConnectionPlayer>();
        rb = GetComponent<Rigidbody>();
    }

    //[ServerCallback]
    void OnEnable()
    {
        weight = maxWeight / 2;
        RpcTakeDamage();
    }

    private void Update()
    {
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    CmdChangeWeight(weightChange);
        //}
        //if (Input.GetButtonDown("Fire2"))
        //{
        //    CmdChangeWeight(-weightChange);
        //}
        //Debug.Log(weight);
    }

    private void FixedUpdate()
    {
        if (weight > 0)
        {
            rb.AddForce(0, -(Mathf.Clamp(weight, minGravityWeight, maxWeight)), 0);
        }
        else
            rb.AddForce(0, -(weight), 0);
    }

    //[Server]
    public void TakeDamage(float balDamage)
    {
        //bool died = false;

        //if (weight <= 0)
        //    return died;

        weight += balDamage;
        //died = weight <= 0;

        RpcTakeDamage();

        //return died;
    }

    //[ClientRpc]
    void RpcTakeDamage()
    {
        txtWeight.text = weight.ToString("F1");
        //if (died)
        //    connectionPlayer.Die();
    }
}