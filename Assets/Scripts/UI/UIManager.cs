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
        _UILoading.SetActive(false);
        _UIConnection.SetActive(true);
    }

    public void DisableLoading()
    {
        _UILoading.SetActive(false);
    }
    #endregion

    #region UI_MainMenu
    public void TutorielClick()
    {
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
        _UIMainMenu.SetActive(false);
        _UIConnection.SetActive(true);
    }

    public void OptionsClick()
    {
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
        gameManager = GameObject.Instantiate(_gameManager).GetComponent<GameManager>();
        gameManager.CurrentLevel = 1;       
        NetworkManager.singleton.onlineScene = "LevelMulti1";
        NetworkManager.singleton.StartHost();
        _UIConnection.SetActive(false);
    }

    public void JoinClick()
    {
        NetworkManager.singleton.networkAddress = ip.text;
        NetworkManager.singleton.networkPort = int.Parse(port.text);
        NetworkManager.singleton.onlineScene = "LevelMulti1";
        NetworkManager.singleton.StartClient();
        _UIConnection.SetActive(false);
        _UILoading.SetActive(true);
    }

    public void BackConnectionClick()
    {
        _UIConnection.SetActive(false);
        _UIMainMenu.SetActive(true);
    }
    #endregion

    #region UI_Options
    public void BackOptionClick()
    {
        _UIOptions.SetActive(false);
        _UIMainMenu.SetActive(true);
    }
    #endregion
}
