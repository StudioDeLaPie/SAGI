using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Corridor : NetworkBehaviour//MonoBehaviour//
{
    public struct PlayerPositionInCorridor
    {
        public NetworkConnection playerConnection;
        public Vector3 position;
        public Vector3 direction;
        public PlayerPositionInCorridor(NetworkConnection connection, Vector3 pos, Vector3 dir)
        {
            playerConnection = connection;
            position = pos;
            direction = dir;
        }
    }

    public bool isEntry;
    public bool isExit;
    private bool isLocked { get { return !isEntry && !isExit; } }

    [SerializeField] private Animator doorAnimator;
    public List<GameObject> deactivateIfLocked;

    private List<PlayerMovementController> playersInside;
    [HideInInspector] public UnityEvent allPlayersInsideEvent;
    private int nbPlayers = 1;

    public bool IsReady()
    {
        return doorAnimator.isActiveAndEnabled && ClientScene.ready;
    }

    public void SetNbPlayers(int nbPlayers)
    {
        this.nbPlayers = nbPlayers;
    }

    // Use this for initialization
    void Awake()
    {
        if (isLocked)
        {
            GetComponent<BoxCollider>().enabled = false;
            Destroy(GetComponent<NetworkAnimator>());
            foreach(GameObject go in deactivateIfLocked)
            {
                go.SetActive(false);
            }
            enabled = false;
        }
        else
        {
            playersInside = new List<PlayerMovementController>();
        }
    }

    #region Door Management
    public void OpenDoor()
    {
        ChangeDoorState(true);
    }
    public void CloseDoor()
    {
        ChangeDoorState(false);
    }

    private void ChangeDoorState(bool open)
    {
        doorAnimator.SetBool("open", open);
    }
    #endregion

    public bool AllPlayersInside()
    {
        return playersInside.Count == nbPlayers;
    }

    public List<PlayerPositionInCorridor> GetPlayersLocalCoordinates()
    {
        List<PlayerPositionInCorridor> result = new List<PlayerPositionInCorridor>();

        foreach (PlayerMovementController player in playersInside)
        {
            PlayerPositionInCorridor playerPosition = new PlayerPositionInCorridor();

            playerPosition.playerConnection = player.GetComponent<NetworkIdentity>().connectionToClient;
            playerPosition.position = transform.InverseTransformPoint(player.transform.position);
            playerPosition.direction = transform.InverseTransformPoint(playersInside[0].transform.TransformPoint(Vector3.forward));
            result.Add(playerPosition);
        }
        return result;
    }

    public void SetPlayersLocalCoordinates(List<PlayerPositionInCorridor> playersCoor)
    {
        foreach (PlayerPositionInCorridor playerInCor in playersCoor)
        {
            playerInCor.position.Set(-playerInCor.position.x, playerInCor.position.y, -playerInCor.position.z);
            playerInCor.direction.Set(-playerInCor.direction.x, playerInCor.direction.y, -playerInCor.direction.z);
            NetworkIdentity playerNetId = playerInCor.playerConnection.playerControllers[0].unetView;
            playerNetId.GetComponent<ConnectionPlayer>().RpcSetPositionAndDirection(transform.TransformPoint(playerInCor.position), transform.TransformPoint(playerInCor.direction));
            //playerNetId.GetComponent<ConnectionPlayer>().RpcSetPositionAndDirection(Vector3.up, Vector3.forward);
        }
    }


    #region Trigger
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovementController controller = other.GetComponent<PlayerMovementController>();
        if (controller != null)
        {
            playersInside.Add(controller);
            if (AllPlayersInside())
            {
                allPlayersInsideEvent.Invoke();
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
    #endregion
}
