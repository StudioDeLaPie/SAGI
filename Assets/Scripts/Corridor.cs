using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Corridor : MonoBehaviour//NetworkBehaviour
{

    public bool isEntry;
    public bool isExit;
    private bool isLocked { get { return !isEntry && !isExit; } }

    [SerializeField] private Animator entryDoorAnimator;
    [SerializeField] private Animator exitDoorAnimator;

    private List<PlayerMovementController> playersInside;
    [HideInInspector] public UnityEvent bothPlayersInsideEvent;


    // Use this for initialization
    void Awake()
    {
        entryDoorAnimator.SetBool("open", false);
        exitDoorAnimator.SetBool("open", false);
        if (isLocked)
        {
            GetComponent<BoxCollider>().enabled = false;
            Destroy(GetComponent<NetworkAnimator>());
            enabled = false;
            return;
        }
        if (isEntry)
            GetComponent<NetworkAnimator>().animator = exitDoorAnimator;
        else
            GetComponent<NetworkAnimator>().animator = entryDoorAnimator;
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        playersInside = new List<PlayerMovementController>();
    }

    private void Start()
    {
        //if (!isServer)
        //    enabled = false;

    }

    #region Door Management
    public void EntryDoorState(bool open)
    {
        ChangeDoorState(true, open);
    }

    public void ExitDoorState(bool open)
    {
        ChangeDoorState(false, open);
    }

    private void ChangeDoorState(bool isEntry, bool open)
    {
        if (isEntry)
            entryDoorAnimator.SetBool("open", open);
        else
            exitDoorAnimator.SetBool("open", open);
    }
    #endregion

    public bool BothPlayersInside()
    {
        return playersInside.Count == 2;
    }

    public Dictionary<NetworkConnection, Vector3> GetPlayersLocalCoordinates()
    {
        Dictionary<NetworkConnection, Vector3> result = new Dictionary<NetworkConnection, Vector3>();

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            NetworkConnection netId = player.GetComponent<NetworkIdentity>().connectionToClient;
            //if (netId.isServer)
                result.Add(netId, transform.InverseTransformPoint(player.transform.position));
            //else
                //result.Add(netId, transform.InverseTransformPoint(player.GetComponent<ConnectionPlayer>().RpcGetPosition()));
        }
        return result;
    }

    public void SetPlayersLocalCoordinates(Dictionary<NetworkConnection, Vector3> playersCoor)
    {
        foreach (KeyValuePair<NetworkConnection, Vector3> pair in playersCoor)
        {
            if (pair.Key.playerControllers[0].unetView.isServer)
                pair.Key.playerControllers[0].unetView.GetComponent<Transform>().position = transform.TransformPoint(pair.Value);
            else
                pair.Key.playerControllers[0].unetView.GetComponent<ConnectionPlayer>().RpcSetPosition(transform.TransformPoint(pair.Value));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovementController controller = other.GetComponent<PlayerMovementController>();
        if (controller != null)
        {
            playersInside.Add(controller);
            if (BothPlayersInside())
            {
                bothPlayersInsideEvent.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovementController controller = other.GetComponent<PlayerMovementController>();
        if (controller != null)
        {
            playersInside.Remove(controller);
        }
    }
    
    public bool IsReady()
    {
        return entryDoorAnimator.isActiveAndEnabled && exitDoorAnimator.isActiveAndEnabled;
    }
}
