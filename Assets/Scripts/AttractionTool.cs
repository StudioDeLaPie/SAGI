using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionTool : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject prefabFaceOverlay;
    private GameObject faceOverlay;
    private BaseWeapon weapon;
    private Weight playerWeight;
    private PlayerMovementController movementController;

    private void Start()
    {
        faceOverlay = Instantiate(prefabFaceOverlay);
        weapon = GetComponent<BaseWeapon>();
        playerWeight = GetComponent<Weight>();
        movementController = GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (weapon.isActiveAndEnabled)
                weapon.enabled = false;
            RaycastHit hit;
            Ray ray = new Ray(firePoint.position, firePoint.forward);
            bool result = Physics.Raycast(ray, out hit, 6f);

            if (result)
            {
                Weight touchedObject = hit.transform.GetComponent<Weight>();
                if (touchedObject != null)
                {
                    UpdateOverlay(hit);
                    if (Input.GetMouseButtonDown(0))
                    {
                        touchedObject.Repulsion(hit.normal);
                        movementController.DisableAirControl(2);
                        playerWeight.Attraction(hit.normal);
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        touchedObject.Attraction(hit.normal);
                        movementController.DisableAirControl(2);
                        playerWeight.Repulsion(hit.normal);
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

    void UpdateOverlay(RaycastHit hit)
    {
        faceOverlay.SetActive(true);

        faceOverlay.transform.position = hit.transform.position;

        Vector3 temp = hit.normal;
        Vector3 rotation = Vector3.zero;

        if (temp == Vector3.up)
            rotation = new Vector3(-90, 0, 0);
        else if (temp == Vector3.down)
            rotation = new Vector3(90, 0, 0);
        else if (temp == Vector3.forward)
            rotation = Vector3.zero;
        else if (temp == Vector3.back)
            rotation = new Vector3(0, 180, 0);
        else if (temp == Vector3.left)
            rotation = new Vector3(0, -90, -90);
        else if (temp == Vector3.right)
            rotation = new Vector3(0, 90, 90);

        faceOverlay.transform.eulerAngles = rotation;

        faceOverlay.transform.localScale = hit.transform.localScale;
    }
}
