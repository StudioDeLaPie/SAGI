using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class BaseWeapon : NetworkBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private Transform firePoint;
    //[SerializeField] private ShotEffectsManager shotEffects;
    private RaycastHit hit;
    private Weight touchedObject;
    private float lastShot;

    private void Start()
    {
        lastShot = Time.time;
    }

    private void Update()
    {
        if (Time.time > lastShot + cooldown)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                LocalFire(true);               
            }

            if (Input.GetButtonDown("Fire2"))
            {
                LocalFire(false);
            }

            if (Input.GetButtonDown("Freeze"))
            {
                LocalFreeze();
            }
        }
        if (Input.GetButtonDown("Stop"))
        {
            LocalStop();
        }
    }

    private void LocalFire(bool changeWeightPositively)
    {
        if (CalculateRayCast())
        {
            if (touchedObject != null)
            {
                CmdFire(touchedObject.GetComponent<NetworkIdentity>(), changeWeightPositively);
                //UpdateMaterialIfCube(touchedObject);
            }
        }
        lastShot = Time.time;
        //RpcProcessShotEffects(result, hit.point);
    }

    [Command]
    private void CmdFire(NetworkIdentity touchedObjectId, bool changeWeightPositively)
    {
        if (changeWeightPositively)
            touchedObjectId.GetComponent<Weight>().CmdIncreaseWeight();
        else
            touchedObjectId.GetComponent<Weight>().CmdDecreaseWeight();

    }

    private void LocalStop()
    {
        if (CalculateRayCast())
        {
            if (touchedObject != null)
            {
                CmdStop(touchedObject.GetComponent<NetworkIdentity>());

                //UpdateMaterialIfCube(touchedObject);
            }
        }
        lastShot = Time.time;
    }

    [Command]
    private void CmdStop(NetworkIdentity touchedObjectId)
    {
        touchedObjectId.GetComponent<Weight>().CmdStop();
    }

    private void LocalFreeze()
    {
        if (CalculateRayCast())
        {
            if (touchedObject != null)
            {
                CmdFreeze(touchedObject.GetComponent<NetworkIdentity>());

                //UpdateMaterialIfCube(touchedObject);
            }
        }
        lastShot = Time.time;
    }

    [Command]
    private void CmdFreeze(NetworkIdentity touchedObjectId)
    {
        touchedObjectId.GetComponent<Weight>().CmdFreeze();
    }

    private bool CalculateRayCast()
    {
        bool result = Physics.Raycast(firePoint.position, firePoint.forward, out hit, 500f, layerMask.value);
        if (result)
            touchedObject = hit.transform.GetComponent<Weight>();
        return result;
    }

    /*[ClientRpc]
    void RpcProcessShotEffects(bool playImpact, Vector3 point)
    {
        shotEffects.PlayShotEffects();
        if (playImpact)
            shotEffects.PlayImpactEffect(point);
    }*/
}
