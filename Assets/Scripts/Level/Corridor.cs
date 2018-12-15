using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Corridor : NetworkBehaviour
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
    [SerializeField] private Light doorLight;
    public List<GameObject> deactivateIfLocked;

    private List<PlayerMovementController> playersInside;
    [HideInInspector] public UnityEvent allPlayersInsideEvent;
    private bool doorClosed = false;

    public bool IsReady()
    {
        return doorAnimator.isActiveAndEnabled && ClientScene.ready;
    }

    private int nbPlayers = 1;
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
            Destroy(doorLight.gameObject);
            foreach (GameObject go in deactivateIfLocked)
            {
                go.SetActive(false);
            }
            enabled = false;
        }
        else
        {
            playersInside = new List<PlayerMovementController>();
            if (isExit)
                doorLight.color = new Color(90, 0, 0);
            if (isEntry)
                doorLight.color = new Color(0, 64, 168);
        }
    }

    #region Door Management
    public void OpenDoor()
    {
        ChangeDoorState(true);
        doorClosed = false;
    }
    public void CloseDoor()
    {
        doorClosed = true;
        ChangeDoorState(false);
    }

    private void ChangeDoorState(bool open)
    {
        if (isExit)
        {
            if (open)
                doorLight.color = new Color(0, 90, 0);
            else
                doorLight.color = new Color(90, 0, 0);
        }
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
        if (!doorClosed)
        {
            PlayerMovementController controller = other.GetComponent<PlayerMovementController>();
            if (controller != null)
            {
                playersInside.Remove(controller);
            }
        }
    }
    #endregion
}
