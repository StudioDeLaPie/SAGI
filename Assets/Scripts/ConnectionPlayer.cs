using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }

public class ConnectionPlayer : NetworkBehaviour
{
    [SerializeField] ToggleEvent onToggleShared;
    [SerializeField] ToggleEvent onToggleLocal;
    [SerializeField] ToggleEvent onToggleRemote;
    [SerializeField] float respawnTime = 5f;

    GameObject mainCamera;

    void Start()
    {
        /*if (isLocalPlayer)
            mainCamera = Camera.main.gameObject;
            */
        EnablePlayer();
        DontDestroyOnLoad(gameObject);
    }

    void DisablePlayer()
    {
        //if (isLocalPlayer)
            //mainCamera.SetActive(true);

        onToggleShared.Invoke(false);

        if (isLocalPlayer)
            onToggleLocal.Invoke(false);
        else
            onToggleRemote.Invoke(false);
    }

    void EnablePlayer()
    {
        //if (isLocalPlayer)
            //mainCamera.SetActive(false);

        onToggleShared.Invoke(true);

        if (isLocalPlayer)
        {
            onToggleLocal.Invoke(true);
            onToggleRemote.Invoke(false);
        }
        else
        {
            onToggleLocal.Invoke(false);
            onToggleRemote.Invoke(true);
        }
    }

    //[ClientRpc]
    //public Vector3 RpcGetPosition()
    //{
    //    return transform.position;
    //}

    [ClientRpc]
    public void RpcSetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void Die()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        DisablePlayer();

        Invoke("Respawn", respawnTime);
    }

    void Respawn()
    {
        if (isLocalPlayer)
        {
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
        }

        EnablePlayer();
    }

    public void Disconnect()
    {
        if(!isServer)
        {
            NetworkManager.singleton.StopClient();
        }
        else if(isServer)
        {
            NetworkManager.singleton.StopServer();
        }
    }
}