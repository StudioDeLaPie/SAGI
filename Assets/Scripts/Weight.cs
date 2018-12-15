using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Weight : NetworkBehaviour
{
    private int maxWeight = 2;
    private int minWeight = -2;
    [SerializeField] private float weightMultiplier = 200;

    //ConnectionPlayer connectionPlayer;

    [SerializeField, SyncVar] private int currentWeight;
    
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
        if (gameObject.tag == "Player" && isServer)
        {
            currentWeight = 1;
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(0, -(currentWeight * weightMultiplier), 0);
    }

    [Command]
    public void CmdIncreaseWeight()
    {
        RpcSetKinematic(false);
        currentWeight++;
        currentWeight = Mathf.Clamp(currentWeight, minWeight, maxWeight);
        //RpcChangeWeightSound(true);
    }

    [Command]
    public void CmdDecreaseWeight()
    {
        RpcSetKinematic(false);
        currentWeight--;
        currentWeight = Mathf.Clamp(currentWeight, minWeight, maxWeight);
        //RpcChangeWeightSound(false);
    }

    [Command]
    public void CmdStop()
    {
        currentWeight = 0;
        RpcStop();
    }

    [Command]
    public void CmdFreeze()
    {
        currentWeight = 0;
        RpcFreeze();
    }

    [ClientRpc]
    private void RpcStop()
    {
        rb.isKinematic = true;
        rb.isKinematic = false;
        //GetComponent<WeightSoundManager>().ShotStop();
    }

    [ClientRpc]
    private void RpcFreeze()
    {
        rb.isKinematic = true;
        //GetComponent<WeightSoundManager>().ShotFreeze();
    }

    [ClientRpc]
    private void RpcSetKinematic(bool isKinematic)
    {
        rb.isKinematic = isKinematic;
    }

    //[ClientRpc]
    //private void RpcChangeWeightSound(bool increased)
    //{
    //    if (increased)
    //        GetComponent<WeightSoundManager>().ShotIncreasse();
    //    else
    //        GetComponent<WeightSoundManager>().ShotDecreasse();
    //}

    /// <summary>
    /// Demande a l'entite de s'appliquer une force d'ejection dans la direction donnee
    /// </summary>
    [ClientRpc]
    public void RpcRepulsion(Vector3 hitNormal)
    {
        Repulsion(hitNormal);
    }
    public void Repulsion(Vector3 hitNormal)
    {
        AttractionRepulsion(hitNormal, false);
    }

    [ClientRpc]
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
}