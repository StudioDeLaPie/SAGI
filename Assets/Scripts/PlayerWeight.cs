using UnityEngine;
using UnityEngine.UI;

public class PlayerWeight : MonoBehaviour
{
    private int maxWeight = 2;
    private int minWeight = -2;
    [SerializeField] private float weightMultiplier = 200;

    //ConnectionPlayer connectionPlayer;
    [SerializeField] private int weight; //syncvar
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
        RpcUpdateDisplay();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            IncreaseWeight();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            DecreaseWeight();
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(0, -(weight * weightMultiplier), 0);
    }

    public void IncreaseWeight()
    {
        ChangeWeight(true);
    }

    public void DecreaseWeight()
    {
        ChangeWeight(false);
    }

    private void ChangeWeight(bool positively)
    {
        weight = positively ? weight + 1 : weight - 1;
        weight = Mathf.Clamp(weight, minWeight, maxWeight);
        RpcUpdateDisplay();
    }

    //[Server]
    //public void TakeDamage(float balDamage)
    //{
    //    //bool died = false;

    //    //if (weight <= 0)
    //    //    return died;

    //    weight += balDamage;
    //    //died = weight <= 0;

    //    RpcTakeDamage();

    //    //return died;
    //}

    //[ClientRpc]
    void RpcUpdateDisplay()
    {
        txtWeight.text = weight.ToString();
        //if (died)
        //    connectionPlayer.Die();
    }
}