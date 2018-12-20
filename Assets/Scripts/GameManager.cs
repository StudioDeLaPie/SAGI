using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private int currentLevel = 1;
    private string nameSceneMulti = "LevelMulti";
    private string nameSceneSolo = "Level";

    [HideInInspector] public List<Corridor.PlayerPositionInCorridor> playersCoordinatesInCorridor = null;

    public bool multi;

    public int CurrentLevel
    {
        get
        {
            return currentLevel;
        }

        set
        {
            currentLevel = value;
        }
    }

    public bool Multi
    {
        get
        {
            return multi;
        }

        set
        {
            multi = value;
        }
    }

    private void Awake()
    {        
        if (instance == null)
        {
            instance = this;            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        multi = LobbyManager.s_Singleton.multi;
    }

    public void LoadNextLevel()
    {
        if (currentLevel != 5)
        {
            multi = LobbyManager.s_Singleton.multi;
            int temp = currentLevel + 1;
            NetworkManager.singleton.ServerChangeScene((Multi ? nameSceneMulti : nameSceneSolo) + temp);
            currentLevel++;
        }
        else
        {
            NetworkManager.singleton.ServerChangeScene(("FinalScreen"));
        }
    }

    public void ResetLevel()
    {
        NetworkManager.singleton.ServerChangeScene((Multi ? nameSceneMulti : nameSceneSolo) + currentLevel);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        playersCoordinatesInCorridor = null;
        currentLevel = 1;
    }
}
