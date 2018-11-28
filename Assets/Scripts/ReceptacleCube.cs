using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReceptacleCube : MonoBehaviour {

    public bool activated;
    [HideInInspector] public UnityEvent activateEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Jumpable")
        {
            activated = true;
            activateEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Jumpable")
        {
            activated = false;
            activateEvent.Invoke();
        }
    }

    
}
