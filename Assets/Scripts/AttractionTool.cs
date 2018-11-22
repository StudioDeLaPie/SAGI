using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AttractionTool : NetworkBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject prefabFaceOverlay;
    private RaycastHit hit;
    private Vector3 hitNormal;
    private Weight touchedObject;
    private GameObject faceOverlay;
    private BaseWeapon weapon;
    private Weight playerWeight;
    private PlayerMovementController movementController;


    private void Start()
    {
        if (!isLocalPlayer)
            return;

        faceOverlay = Instantiate(prefabFaceOverlay);
        weapon = GetComponent<BaseWeapon>();
        playerWeight = GetComponent<Weight>();
        movementController = GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (weapon.isActiveAndEnabled)
                weapon.enabled = false;

            if (CalculateRaycast())
            {
                if (touchedObject != null)
                {
                    CalculateHitNormal();

                    UpdateOverlay();
                    if (Input.GetMouseButtonDown(0))
                    {
                        movementController.DisableAirControl(2);
                        playerWeight.Attraction(hitNormal);
                        CmdRepulsion(touchedObject.GetComponent<NetworkIdentity>(), hitNormal);
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        movementController.DisableAirControl(2);
                        playerWeight.Repulsion(hitNormal);
                        CmdAttraction(touchedObject.GetComponent<NetworkIdentity>(), hitNormal);
                    }
                }
                else
                {
                    faceOverlay.SetActive(false);
                }
            }
            else
            {
                faceOverlay.SetActive(false);
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            weapon.enabled = true;
            faceOverlay.SetActive(false);
        }
    }

    [Command]
    private void CmdAttraction(NetworkIdentity touchedObjectId, Vector3 hitNormal)
    {
        PlayerMovementController touchedController = touchedObjectId.GetComponent<PlayerMovementController>();

        //Ce n'est pas un joueur ou alors c'est le serveur lui-meme
        if (touchedController == null || touchedObjectId.netId == gameObject.GetComponent<NetworkIdentity>().netId)
        {
            touchedObjectId.GetComponent<Weight>().Attraction(hitNormal);
            if (touchedController != null) //S'il s'agit du serveur
                touchedObjectId.GetComponent<PlayerMovementController>().DisableAirControl(2);
        }
        else //il s'agit d'un autre joueur
        {
            touchedObjectId.GetComponent<Weight>().RpcAttraction(hitNormal);
            touchedObjectId.GetComponent<PlayerMovementController>().RpcDisableAirControl(2);
        }
    }

    [Command]
    private void CmdRepulsion(NetworkIdentity touchedObjectId, Vector3 hitNormal)
    {
        PlayerMovementController touchedController = touchedObjectId.GetComponent<PlayerMovementController>();

        //Ce n'est pas un joueur ou alors c'est le serveur lui-meme
        if (touchedController == null || touchedObjectId.netId == gameObject.GetComponent<NetworkIdentity>().netId)
        {
            touchedObjectId.GetComponent<Weight>().Repulsion(hitNormal);
            if (touchedController != null) //S'il s'agit du serveur
                touchedObjectId.GetComponent<PlayerMovementController>().DisableAirControl(2);
        }
        else //il s'agit d'un autre joueur
        {
            touchedObjectId.GetComponent<Weight>().RpcRepulsion(hitNormal);
            touchedObjectId.GetComponent<PlayerMovementController>().RpcDisableAirControl(2);
        }
    }

    void UpdateOverlay()
    {
        faceOverlay.transform.position = hit.transform.position;
        Vector3 rotation = Vector3.zero;

        if (hitNormal == Vector3.up)
            rotation = new Vector3(-90, 0, 0);
        else if (hitNormal == Vector3.down)
            rotation = new Vector3(90, 0, 0);
        else if (hitNormal == Vector3.forward)
            rotation = Vector3.zero;
        else if (hitNormal == Vector3.back)
            rotation = new Vector3(0, 180, 0);
        else if (hitNormal == Vector3.left)
            rotation = new Vector3(0, -90, -90);
        else if (hitNormal == Vector3.right)
            rotation = new Vector3(0, 90, 90);

        faceOverlay.transform.eulerAngles = rotation;
        faceOverlay.transform.localScale = hit.transform.localScale;

        faceOverlay.SetActive(true);
    }

    private void CalculateHitNormal()
    {
        hitNormal = hit.normal;
        Vector3 absNormal = new Vector3(Mathf.Abs(hit.normal.x), Mathf.Abs(hit.normal.y), Mathf.Abs(hit.normal.z));
        hitNormal = new Vector3
        {
            x = absNormal.x > absNormal.y && absNormal.x > absNormal.z ? 1 : 0,
            y = absNormal.y > absNormal.x && absNormal.y > absNormal.z ? 1 : 0,
            z = absNormal.z > absNormal.x && absNormal.z > absNormal.y ? 1 : 0
        };
        hitNormal = Vector3.Scale(hitNormal, hit.normal).normalized;
    }

    private bool CalculateRaycast()
    {
        bool result = Physics.Raycast(firePoint.position, firePoint.forward, out hit, 6f);
        if (result)
            touchedObject = hit.transform.GetComponent<Weight>();
        return result;
    }
}
