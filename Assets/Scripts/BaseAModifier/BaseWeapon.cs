using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class BaseWeapon : MonoBehaviour
{
    private float cooldown = 0.5f;

    [SerializeField] private Transform firePoint;
    //[SerializeField] private ShotEffectsManager shotEffects;
    private float lastShot;

    public float Cooldown { get { return cooldown; } set { cooldown = value; } }
    public Transform FirePoint { get { return firePoint; } }

    public void Initialize()
    {
        lastShot = Time.time;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Fire(false);

        else if (Input.GetMouseButtonDown(1))
            Fire(true);

        if (Input.GetKeyDown(KeyCode.E))
            Stop();

        if (Input.GetKeyDown(KeyCode.R))
            Freeze();
    }

    private void Fire(bool mouseCode)//, bool clientHit, NetworkInstanceId id)
    {
        //Debug.Log(NetworkManager.singleton.client.GetRTT());
        //NetworkIdentity client = NetworkServer.FindLocalObject(id).GetComponent<NetworkIdentity>().clientAuthorityOwner.;
        //Debug.Log(client.GetRTT());

        if (Time.time <= lastShot + cooldown)
            return;

        RaycastHit hit;
        Ray ray = new Ray(firePoint.position, firePoint.forward);

        Debug.DrawRay(firePoint.position, firePoint.forward, Color.red, 1f);
        bool result = Physics.Raycast(ray, out hit, 500f);

        Weight touchedObject = hit.transform.GetComponent<Weight>();

        if (touchedObject != null)
        {
            hit.collider.GetComponent<Rigidbody>().isKinematic = false;

            if (mouseCode)
                touchedObject.DecreaseWeight();

            else if (mouseCode)
                touchedObject.IncreaseWeight();

            if (touchedObject.transform.tag == "Cube")
                touchedObject.GetComponent<MaterialManager>().UpdateMaterial();
        }

        lastShot = Time.time;
        //RpcProcessShotEffects(result, hit.point);
    }

    private void Stop()
    {
        if (Time.time <= lastShot + cooldown)
            return;

        RaycastHit hit;
        Ray ray = new Ray(firePoint.position, firePoint.forward);

        Debug.DrawRay(firePoint.position, firePoint.forward, Color.red, 1f);
        bool result = Physics.Raycast(ray, out hit, 500f);

        Weight touchedObject = hit.transform.GetComponent<Weight>();

        if (touchedObject != null)
        {
            hit.collider.GetComponent<Rigidbody>().isKinematic = false;

            touchedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            touchedObject.GetComponent<Weight>().ZeroGravity();

            if (touchedObject.transform.tag == "Cube")
                touchedObject.GetComponent<MaterialManager>().UpdateMaterial();
        }

        lastShot = Time.time;
    }

    private void Freeze()
    {
        if (Time.time <= lastShot + cooldown)
            return;

        RaycastHit hit;
        Ray ray = new Ray(firePoint.position, firePoint.forward);

        Debug.DrawRay(firePoint.position, firePoint.forward, Color.red, 1f);
        bool result = Physics.Raycast(ray, out hit, 500f);

        Weight touchedObject = hit.transform.GetComponent<Weight>();

        if (touchedObject != null)
        {
            hit.collider.GetComponent<Rigidbody>().isKinematic = true;

            if (touchedObject.transform.tag == "Cube")
                touchedObject.GetComponent<MaterialManager>().UpdateMaterial();
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
