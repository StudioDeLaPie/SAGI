using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReceptacleCube : MonoBehaviour {

    private bool activated;
    [HideInInspector] public UnityEvent activationEvent;

    public bool isActivated
    {
        get { return activated; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Jumpable")
        {
            activated = true;
            activationEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Jumpable")
        {
            activated = false;
            activationEvent.Invoke();
        }
    }

    
}
