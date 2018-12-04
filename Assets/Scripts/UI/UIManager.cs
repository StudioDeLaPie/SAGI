using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _UIMainMenu;
    [SerializeField] private GameObject _UIConnection;
    [SerializeField] private GameObject _UIOptions;
    [SerializeField] private GameObject _UILoading;
    [SerializeField] private GameObject _UIPauseMenu;
    [SerializeField] private GameObject _gameManager;
    [SerializeField] private SoundManager soundManager;

    [SerializeField] private Text ip;
    [SerializeField] private Text port;


    GameManager gameManager;
    string pathGameManager = "Prefabs/GameManager";

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    #region UI_Loading
    public void CancelLoadingClick()
    {
        soundManager.ShotDefaultUISound();
        _UILoading.SetActive(false);
        _UIConnection.SetActive(true);
    }

    //Utilisé par le customNetworckManager pour enlever la fenetre de chargement
    public void DisableLoading()
    {
        _UILoading.SetActive(false);
    }
    #endregion

    #region UI_MainMenu
    public void TutorielClick()
    {
        soundManager.ShotConnectionUISound();
        gameManager = GameObject.Instantiate(_gameManager).GetComponent<GameManager>();
        gameManager.CurrentLevel = 1;
        NetworkManager.singleton.networkAddress = "127.0.0.1";
        NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.onlineScene = "Level1";
        NetworkManager.singleton.maxConnections = 1;
        NetworkManager.singleton.StartHost();
        _UIMainMenu.SetActive(false);
    }

    public void MultiplayerClick()
    {
        soundManager.ShotDefaultUISound();
        _UIMainMenu.SetActive(false);
        _UIConnection.SetActive(true);
    }

    public void OptionsClick()
    {
        soundManager.ShotDefaultUISound();
        _UIMainMenu.SetActive(false);
        _UIOptions.SetActive(true);
    }

    public void ExitClick()
    {
        Application.Quit();
    }

    #endregion

    #region UI_Connection
    public void HostClick()
    {
        soundManager.ShotConnectionUISound();
        gameManager = GameObject.Instantiate(_gameManager).GetComponent<GameManager>();
        gameManager.CurrentLevel = 1;
        gameManager.Multi = true;
        NetworkManager.singleton.onlineScene = "LevelMulti1";
        NetworkManager.singleton.StartHost();
        _UIConnection.SetActive(false);
    }

    public void OnError(NetworkMessage msg)
    {
        //msg.ReadMessage<ErrorMessage>();
        Debug.Log(msg.ReadMessage<ErrorMessage>().ToString());

    }


    public void JoinClick()
    {
        NetworkClient client = new NetworkClient();
        client.Connect("127.0.0.1", 7777);
        client.RegisterHandler(MsgType.Error, OnError);
        soundManager.ShotConnectionUISound();
        NetworkManager.singleton.networkAddress = ip.text;
        NetworkManager.singleton.networkPort = int.Parse(port.text);
        NetworkManager.singleton.onlineScene = "LevelMulti1";
        NetworkManager.singleton.StartClient();
        _UIConnection.SetActive(false);
        _UILoading.SetActive(true);
    }

    public void BackConnectionClick()
    {
        soundManager.ShotDefaultUISound();
        _UIConnection.SetActive(false);
        _UIMainMenu.SetActive(true);
    }
    #endregion

    #region UI_Options
    public void BackOptionClick()
    {
        soundManager.ShotDefaultUISound();
        _UIOptions.SetActive(false);
        _UIMainMenu.SetActive(true);
    }
    #endregion
}
