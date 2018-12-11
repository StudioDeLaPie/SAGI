using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightOffline : MonoBehaviour
{

    private int maxWeight = 2;
    private int minWeight = -2;
    [SerializeField] private float weightMultiplier = 200;


    [SerializeField] private int currentWeight;
    private Rigidbody rb;
    private MaterialManagerOffline materialManager;

    private void Awake()
    {
        //connectionPlayer = GetComponent<ConnectionPlayer>();
        rb = GetComponent<Rigidbody>();
        materialManager = GetComponent<MaterialManagerOffline>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(0, -(currentWeight * weightMultiplier), 0);
    }


    public void IncreaseWeight()
    {
        currentWeight++;
        currentWeight = Mathf.Clamp(currentWeight, minWeight, maxWeight);
        materialManager.UpdateMaterial(currentWeight);
    }


    public void DecreaseWeight()
    {
        currentWeight--;
        currentWeight = Mathf.Clamp(currentWeight, minWeight, maxWeight);
        materialManager.UpdateMaterial(currentWeight);
    }
}