﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCubeOffline : MonoBehaviour
{
    private float cooldown;
    private float lastActionTime;

    private WeightOffline weight;
    private Rigidbody rb;

    private void Awake()
    {
        cooldown = Aleatoire.AleatoireBetweenFloat(0.3f, 1.5F);
        lastActionTime = Time.time;
        weight = GetComponent<WeightOffline>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ChangeWeight();
        Addforce();
    }

    private void Update()
    {
        if(Time.time > cooldown + lastActionTime)
        {
            if(Aleatoire.AleatoireBetween(0, 5) <= 4)
            {
                ChangeWeight();
            }
            if(Aleatoire.AleatoireBetween(0, 5) <= 4)
            {
                /*if (Aleatoire.AleatoireBetween(0, 1) == 1)
                    stop();*/
                Addforce();
            }
            lastActionTime = Time.time;
        }
    }

    private void ChangeWeight()
    {
        if (Aleatoire.AleatoireBetween(0, 1) == 1)
            weight.IncreaseWeight();
        else
            weight.DecreaseWeight();
    }

    private void Addforce()
    {       
        rb.AddForce(Aleatoire.AleatoireVectorDirection() * Aleatoire.AleatoireBetween(1000,1500));
    }
}
