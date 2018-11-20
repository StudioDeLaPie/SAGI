using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private Transform firePoint;
    //[SerializeField] private ShotEffectsManager shotEffects;
    private float lastShot;

    private void Start()
    {
        lastShot = Time.time;
    }

    private void Update()
    {
        if (Time.time > lastShot + cooldown)
        {
            if (Input.GetMouseButtonDown(0))
                Fire(true);

            if (Input.GetMouseButtonDown(1))
                Fire(false);

            if (Input.GetKeyDown(KeyCode.R))
                Freeze();
        }
        if (Input.GetKeyDown(KeyCode.E))
            Stop();
    }

    private void Fire(bool changeWeightPositively)//, bool clientHit, NetworkInstanceId id)
    {
        //Debug.Log(NetworkManager.singleton.client.GetRTT());
        //NetworkIdentity client = NetworkServer.FindLocalObject(id).GetComponent<NetworkIdentity>().clientAuthorityOwner.;
        //Debug.Log(client.GetRTT());

        RaycastHit hit;
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        bool result = Physics.Raycast(ray, out hit, 500f);

        if (result)
        {
            Weight touchedObject = hit.transform.GetComponent<Weight>();

            if (touchedObject != null)
            {
                hit.collider.GetComponent<Rigidbody>().isKinematic = false;

                if (changeWeightPositively)
                    touchedObject.IncreaseWeight();
                else
                    touchedObject.DecreaseWeight();

                if (touchedObject.transform.tag == "Cube")
                    touchedObject.GetComponent<MaterialManager>().UpdateMaterial();
            }
        }

        lastShot = Time.time;
        //RpcProcessShotEffects(result, hit.point);
    }

    private void Stop()
    {
        RaycastHit hit;
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        bool result = Physics.Raycast(ray, out hit, 500f);

        if (result)
        {
            Weight touchedObject = hit.transform.GetComponent<Weight>();
            if (touchedObject != null)
            {
                touchedObject.GetComponent<Rigidbody>().isKinematic = false; //Sort l'objet touché du mode Freeze
                touchedObject.GetComponent<Rigidbody>().velocity = Vector3.zero; //Stop son mouvement
                touchedObject.ZeroGravity(); //Fixe son poids à 0

                if (touchedObject.transform.tag == "Cube")
                    touchedObject.GetComponent<MaterialManager>().UpdateMaterial();
            }
        }
        lastShot = Time.time;
    }

    private void Freeze()
    {
        RaycastHit hit;
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        bool result = Physics.Raycast(ray, out hit, 500f);

        if (result)
        {
            Weight touchedObject = hit.transform.GetComponent<Weight>();
            if (touchedObject != null)
            {
                hit.collider.GetComponent<Rigidbody>().isKinematic = true; //Freeze l'objet
                touchedObject.ZeroGravity(); //Met son poids à zero

                if (touchedObject.transform.tag == "Cube")
                    touchedObject.GetComponent<MaterialManager>().UpdateMaterial();
            }
        }
        lastShot = Time.time;
    }


    /*[ClientRpc]
    void RpcProcessShotEffects(bool playImpact, Vector3 point)
    {
        shotEffects.PlayShotEffects();
        if (playImpact)
            shotEffects.PlayImpactEffect(point);
    }*/
}
