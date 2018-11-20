using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionTool: MonoBehaviour {
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject prefabFaceOverlay;
    private GameObject faceOverlay;

    private void Start()
    {
        faceOverlay = Instantiate(prefabFaceOverlay);        
    }

    void Update () {

        RaycastHit hit;
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        bool result = Physics.Raycast(ray, out hit, 6f);

        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(result)
                UpdateOverlay(hit);
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            faceOverlay.SetActive(false);
        }
    }

    void UpdateOverlay(RaycastHit hit)
    {               
        faceOverlay.SetActive(true);
        //Debug.DrawRay(new Vector3(hit.point.x, hit.point.y, hit.point.z), hit.normal, Color.blue);
        //faceOverlay.transform.SetPositionAndRotation(hit.transform.position, new Quaternion(hit.normal.x * 90, hit.normal.y * 90, hit.normal.z * 90, 1));        

        faceOverlay.transform.position = hit.transform.position;
        faceOverlay.transform.eulerAngles = new Vector3(0, 90, 0);

    }
}
