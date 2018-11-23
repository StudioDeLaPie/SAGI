using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightSolo : MonoBehaviour {

    private int maxWeight = 2;
    private int minWeight = -2;
    [SerializeField] private float weightMultiplier = 200;


    [SerializeField] private int currentWeight;
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

    private void Awake()
    {
        //connectionPlayer = GetComponent<ConnectionPlayer>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (gameObject.tag == "Player")
        {
            CmdIncreaseWeight();
            txtWeight.text = currentWeight.ToString();
        }
    }

    private void Update()
    {
        if (gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
                CmdIncreaseWeight();
            else if (Input.GetKeyDown(KeyCode.G))
                CmdDecreaseWeight();
            UpdateDisplay();
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(0, -(currentWeight * weightMultiplier), 0);
    }


    public void CmdIncreaseWeight()
    {
        RpcSetKinematic(false);
        currentWeight++;
        currentWeight = Mathf.Clamp(currentWeight, minWeight, maxWeight);

    }


    public void CmdDecreaseWeight()
    {
        RpcSetKinematic(false);
        currentWeight--;
        currentWeight = Mathf.Clamp(currentWeight, minWeight, maxWeight);

    }


    public void CmdStop()
    {
        RpcSetKinematic(true);
        RpcSetKinematic(false);
        currentWeight = 0;

    }


    public void CmdFreeze()
    {
        CmdStop(); //Set le poids a zero et update l'affichage
        RpcSetKinematic(true); //Freeze l'objet
    }


    private void RpcSetKinematic(bool isKinematic)
    {
        if (isKinematic)
            rb.velocity = Vector3.zero;
        rb.isKinematic = isKinematic;
    }

    /// <summary>
    /// Demande a l'entite de s'appliquer une force d'ejection dans la direction donnee
    /// </summary>

    public void RpcRepulsion(Vector3 hitNormal)
    {
        Repulsion(hitNormal);
    }
    public void Repulsion(Vector3 hitNormal)
    {
        AttractionRepulsion(hitNormal, false);
    }


    public void RpcAttraction(Vector3 hitNormal)
    {
        Attraction(hitNormal);
    }

    public void Attraction(Vector3 hitNormal)
    {
        AttractionRepulsion(hitNormal, true);
    }

    private void AttractionRepulsion(Vector3 hitNormal, bool isAttracting)
    {
        hitNormal = isAttracting ? hitNormal * 150f : hitNormal * -150f; //force de propulsion

        rb.velocity = Vector3.zero;
        rb.AddForce(hitNormal, ForceMode.Impulse); //Application de la force
    }

    private void UpdateDisplay()
    {
        if (gameObject.transform.tag == "Player")
            txtWeight.text = currentWeight.ToString();
    }


}