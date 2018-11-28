using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couloir : MonoBehaviour
{

    public bool isEntry;
    public bool isExit;
    private bool isLocked { get { return !isEntry && !isExit; } }

    [SerializeField] private Animator entryDoorAnimator;
    [SerializeField] private Animator exitDoorAnimator;

    private List<PlayerMovementController> playersInside;

    // Use this for initialization
    void Start()
    {
        if (isLocked)
        {
            GetComponent<BoxCollider>().enabled = false;
            return;
        }
        playersInside = new List<PlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocked)
            return;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovementController controller = other.GetComponent<PlayerMovementController>();
        if (controller != null)
        {
            playersInside.Add(controller);
            entryDoorAnimator.SetBool("character_nearby", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovementController controller = other.GetComponent<PlayerMovementController>();
        if (controller != null)
        {
            playersInside.Remove(controller);
            entryDoorAnimator.SetBool("character_nearby", false);
        }
    }
}
