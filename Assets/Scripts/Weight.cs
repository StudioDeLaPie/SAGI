using UnityEngine;
using UnityEngine.UI;

public class Weight : MonoBehaviour
{
    private int maxWeight = 2;
    private int minWeight = -2;
    [SerializeField] private float weightMultiplier = 200;

    //ConnectionPlayer connectionPlayer;

    [SerializeField, Range(-2, 2)] private int currentWeight; //syncvar
    [SerializeField] private Text txtWeight;
    private Rigidbody rb;

    public int CurrentWeight
    {
        get
        {
            return currentWeight;
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
        if (gameObject.tag == "Player")
            currentWeight = 2;

        RpcUpdateDisplay();
    }

    private void Update()
    {
        if (gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
                ChangeWeight(true);
            else if (Input.GetKeyDown(KeyCode.G))
                ChangeWeight(false);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(0, -(currentWeight * weightMultiplier), 0);
    }

    public void IncreaseWeight()
    {
        ChangeWeight(true);
    }

    public void DecreaseWeight()
    {
        ChangeWeight(false);
    }

    public void ZeroGravity()
    {
        currentWeight = 0;
        RpcUpdateDisplay();
    }

    private void ChangeWeight(bool positively)
    {
        currentWeight = positively ? currentWeight + 1 : currentWeight - 1;
        currentWeight = Mathf.Clamp(currentWeight, minWeight, maxWeight);
        RpcUpdateDisplay();
    }

    /// <summary>
    /// Demande a l'entite de s'appliquer une force d'ejection dans la direction donnee
    /// </summary>
    //[ClientRpc]
    public void Repulsion(Vector3 hitNormal)
    {
        AttractionRepulsion(hitNormal, false);
    }

    public void Attraction(Vector3 hitNormal)
    {
        AttractionRepulsion(hitNormal, true);
    }

    private void AttractionRepulsion(Vector3 hitNormal, bool isAttracting)
    {
        hitNormal = isAttracting ? hitNormal* 150f : hitNormal * -150f; //force de propulsion

        rb.velocity = Vector3.zero;
        rb.AddForce(hitNormal, ForceMode.Impulse); //Application de la force
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
        if (gameObject.transform.tag == "Player")
            txtWeight.text = currentWeight.ToString();
        //if (died)
        //    connectionPlayer.Die();
    }


}