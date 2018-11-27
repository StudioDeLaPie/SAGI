using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject connection;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject loading;

    [SerializeField] private Text ip;
    [SerializeField] private Text port;


    GameManager gameManager;

    string pathGameManager = "Prefabs/GameManager";  

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnError(NetworkMessage msg)
    {
        Debug.Log("Timeout");
    }


    #region MainMenu
    public void TutorielClick()
    {
        gameManager = GameObject.Instantiate(Resources.Load<GameObject>(pathGameManager)).GetComponent<GameManager>();
        gameManager.CurrentLevel = 1;
        NetworkManager.singleton.networkAddress = "127.0.0.1";
        NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.onlineScene = "Level1";
        NetworkManager.singleton.maxConnections = 1;
        NetworkManager.singleton.StartHost();
        mainMenu.SetActive(false);
    }

    public void MultiplayerClick()
    {
        mainMenu.SetActive(false);
        connection.SetActive(true);
    }

    public void OptionsClick()
    {
        Debug.Log("Menu non disponible");
    }

    public void ExitClick()
    {
        Application.Quit();
    }

    #endregion

    #region Connection
    public void HostClick()
    {
        gameManager = GameObject.Instantiate(Resources.Load<GameObject>(pathGameManager)).GetComponent<GameManager>();
        gameManager.CurrentLevel = 1;
        mainMenu.SetActive(false);               
        NetworkManager.singleton.onlineScene = "LevelMulti1";
        NetworkManager.singleton.StartHost();
        connection.SetActive(false);
    }

    public void JoinClick()
    {
        gameManager = GameObject.Instantiate(Resources.Load<GameObject>(pathGameManager)).GetComponent<GameManager>();
        gameManager.CurrentLevel = 1;

        NetworkClient client = new NetworkClient();
        client.RegisterHandler(MsgType.Error, OnError);

        client.Connect(ip.text, int.Parse(port.text));


        NetworkManager.singleton.networkAddress = ip.text;
        NetworkManager.singleton.networkPort = int.Parse(port.text);
        NetworkManager.singleton.onlineScene = "LevelMulti1";        
        NetworkManager.singleton.StartClient();        
        connection.SetActive(false);
        loading.SetActive(true);
    }

    public void BackClick()
    {
        connection.SetActive(false);
        mainMenu.SetActive(true);
    }
    #endregion    
}
