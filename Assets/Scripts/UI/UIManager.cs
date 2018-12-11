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
    [SerializeField] private GameObject _PrefabGameManager;
    [SerializeField] private SoundManager soundManager;

    [SerializeField] private Text ip;
    [SerializeField] private Text port;

    GameManager gameManager;

    private GameObject previousUIOptions;

    private GameObject actifMenu;

    private UIPlayerManager _UIlocalPlayer;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        actifMenu = _UIMainMenu;
    }

    #region UI_Loading
    public void CancelLoadingClick()
    {
        soundManager.ShotDefaultUISound();
        _UILoading.SetActive(false);
        _UIConnection.SetActive(true);
        actifMenu = null;
    }

    //Utilisé par le customNetworckManager pour enlever la fenetre de chargement
    public void DisableLoading()
    {
        _UILoading.SetActive(false);
        actifMenu = null;
    }
    #endregion

    #region UI_MainMenu
    public void TutorielClick()
    {
        soundManager.ShotConnectionUISound();
        gameManager = GameObject.Instantiate(_PrefabGameManager).GetComponent<GameManager>();
        gameManager.CurrentLevel = 1;
        NetworkManager.singleton.networkAddress = "127.0.0.1";
        NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.onlineScene = "Level1";
        NetworkManager.singleton.maxConnections = 1;
        NetworkManager.singleton.StartHost();
        SetCusrorVisible(false);
        _UIMainMenu.SetActive(false);
        actifMenu = null;
    }

    public void MultiplayerClick()
    {
        soundManager.ShotDefaultUISound();
        _UIMainMenu.SetActive(false);
        _UIConnection.SetActive(true);
        actifMenu = _UIConnection;
    }


    public void OptionsClick(GameObject previousUI)
    {
        if (previousUI == _UIMainMenu)
        {
            _UIMainMenu.SetActive(false);
        }
        else if (previousUI == _UIPauseMenu)
        {
            _UIPauseMenu.SetActive(false);
        }
        else
            Debug.LogError("L'affichage de l'UI Option ne sait pas d'ou il vient. Renseignez la précedente UI");
        actifMenu = _UIOptions;
        previousUIOptions = previousUI;
        _UIOptions.SetActive(true);
        soundManager.ShotDefaultUISound();

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
        gameManager = GameObject.Instantiate(_PrefabGameManager).GetComponent<GameManager>();
        gameManager.CurrentLevel = 1;
        gameManager.Multi = true;
        NetworkManager.singleton.onlineScene = "LevelMulti1";
        NetworkManager.singleton.maxConnections = 2;
        NetworkManager.singleton.StartHost();
        SetCusrorVisible(false);
        _UIConnection.SetActive(false);
        actifMenu = null;
    }

    public void OnError(NetworkMessage msg)
    {
        //msg.ReadMessage<ErrorMessage>();
        Debug.Log(msg.ReadMessage<ErrorMessage>().ToString());

    }


    public void JoinClick()
    {
        NetworkClient client = new NetworkClient();
        client.Connect(ip.text, int.Parse(port.text));
        client.RegisterHandler(MsgType.Error, OnError);
        soundManager.ShotConnectionUISound();
        NetworkManager.singleton.networkAddress = ip.text;
        NetworkManager.singleton.networkPort = int.Parse(port.text);
        NetworkManager.singleton.onlineScene = "LevelMulti1";
        NetworkManager.singleton.StartClient();
        _UIConnection.SetActive(false);
        _UILoading.SetActive(true);
        actifMenu = _UILoading;
    }

    public void BackConnectionClick()
    {
        soundManager.ShotDefaultUISound();
        _UIConnection.SetActive(false);
        _UIMainMenu.SetActive(true);
        actifMenu = _UIMainMenu;
    }
    #endregion

    #region UI_Options
    public void BackOptionClick()
    {
        if (previousUIOptions == _UIMainMenu)
        {
            _UIMainMenu.SetActive(true);
            actifMenu = _UIMainMenu;
        }
        else if (previousUIOptions == _UIPauseMenu)
        {
            _UIPauseMenu.SetActive(true);
            actifMenu = _UIPauseMenu;
        }
        else
            Debug.LogError("L'affichage de l'UI Option ne sait pas d'ou il vient. Renseignez la précedente UI");


        _UIOptions.SetActive(false);
        soundManager.ShotDefaultUISound();
    }
    #endregion

    #region UI_PauseMenu
    public void DrawPauseMenu()
    {
        if (actifMenu == null) //Si il n'y a aucun menu actif on l'affiche
        {
            _UIPauseMenu.SetActive(true);
            SetCusrorVisible(true);
            LocalPlayer.SetVisibleHUD(false);
            LocalPlayer.PauseMenuisActif = true;
            actifMenu = _UIPauseMenu;
            SetAvtivePlayerComponents(false);
            soundManager.ShotDefaultUISound();
        }
    }

    public void ResumePauseMenuClick()
    {
        _UIPauseMenu.SetActive(false); //Onefface le menuPause        
        SetCusrorVisible(false); //on remmet le lock et on rend invisible le curseur
        LocalPlayer.SetVisibleHUD(true); 
        LocalPlayer.PauseMenuisActif = false;
        actifMenu = null; //On dit à l'UIManager qu'il n'y plus de menu courrant        
        SetAvtivePlayerComponents(true);
        soundManager.ShotDefaultUISound();
    }

    public void GoMainMenuFromPauseMenuClick()
    {
        NetworkIdentity netId = LocalPlayer.GetComponent<NetworkIdentity>();

        if (netId.isServer)
            NetworkManager.singleton.StopHost();
        else
            NetworkManager.singleton.StopClient();
        soundManager.ShotDefaultUISound();

        _UIPauseMenu.SetActive(false);
        _UIMainMenu.SetActive(true);
        actifMenu = _UIMainMenu;
        _UIlocalPlayer.PauseMenuisActif = false;
    }

    public void RestartPauseMenuClick()
    {
        gameManager.ResetLevel();
    }

    #endregion

    private void SetCusrorVisible(bool isVisible)
    {
        if (isVisible)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = isVisible;        
    }

    public GameObject ActifMenu
    {
        get
        {
            return actifMenu;
        }

        set
        {
            actifMenu = value;
        }
    }

    public UIPlayerManager LocalPlayer
    {
        get
        {
            return _UIlocalPlayer;
        }

        set
        {
            _UIlocalPlayer = value;
            SetCusrorVisible(false);
        }
    }

    public void SetAvtivePlayerComponents(bool enable)
    {
        _UIlocalPlayer.GetComponent<PlayerMovementController>().enabled = enable;
        _UIlocalPlayer.GetComponent<BaseWeapon>().enabled = enable;
        _UIlocalPlayer.GetComponent<AttractionTool>().enabled = enable;
    }
}
