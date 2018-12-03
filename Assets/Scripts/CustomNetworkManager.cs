using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private UIManager _UIManager;

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        try
        {

        }
        catch
        {
            Debug.Log("Error NetworkManager");
        }
        finally
        {
            if (!NetworkManager.singleton.clientLoadedScene)
            {
                _UIManager.CancelLoadingClick();
            }
        }

    }

    //public override void OnServerReady(NetworkConnection conn)
    //{
    //    base.OnServerReady(conn);
    //    GameObject player = Instantiate(playerPrefab);
    //    NetworkServer.Spawn(player);
    //    ClientScene.AddPlayer(conn, player.GetComponent<NetworkIdentity>().playerControllerId);
    //}

    public override void OnClientConnect(NetworkConnection conn)
    {
        //base.OnClientConnect(conn);
        _UIManager.DisableLoading();
    }
}
