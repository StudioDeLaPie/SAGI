using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{

    private int currentLevel = 1;
    private string nameSceneMulti = "LevelMulti";
    private string nameSceneSolo = "Level";

    [HideInInspector] public List<Corridor.PlayerPositionInCorridor> playersCoordinatesInCorridor = null;

    private bool multi;

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

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadNextLevel()
    {
        int temp = currentLevel + 1;            
        NetworkManager.singleton.ServerChangeScene((multi ? nameSceneMulti : nameSceneSolo) + temp);
        currentLevel++;
    }

    public void ResetLevel()
    {
        NetworkManager.singleton.ServerChangeScene((multi ? nameSceneMulti : nameSceneSolo) + currentLevel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Rechargement du niveau");
            ResetLevel();
        }
    }
}
