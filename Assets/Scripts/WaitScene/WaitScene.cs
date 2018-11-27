using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitScene : MonoBehaviour
{
    private float cooldown;
    private float lastActionTime;

    private WeightSolo weight;
    private Rigidbody rigidbody;

    private void Start()
    {
        cooldown = Aleatoire.AleatoireBetweenFloat(0.5f, 3);
        lastActionTime = Time.time;
        weight = GetComponent<WeightSolo>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Time.time > cooldown + lastActionTime)
        {
            if(Aleatoire.AleatoireBetween(0,1) == 1)
            {
                ChangeWeight();
            }
            if(Aleatoire.AleatoireBetween(0, 1) == 1)
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
            weight.CmdIncreaseWeight();
        else
            weight.CmdDecreaseWeight();
    }

    private void Addforce()
    {       
        rigidbody.AddForce(Aleatoire.AleatoireVectorDirection() * Aleatoire.AleatoireBetween(1000,1500));
    }

    private void stop()
    {
        rigidbody.velocity = Vector3.zero;
    }
}
