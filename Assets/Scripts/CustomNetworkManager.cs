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

    public override void OnClientConnect(NetworkConnection conn)
    {
        _UIManager.DisableLoading();
    }
}
