using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator : MonoBehaviour {

    public GameObject objectToActivate;
    public GameObject activationObject;

	// Use this for initialization
	void Start () {
        objectToActivate.SetActive(false);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject == activationObject)
        {
            objectToActivate.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.gameObject == activationObject)
        {
            objectToActivate.SetActive(false);
        }
    }


}
