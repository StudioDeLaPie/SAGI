using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{

    private int currentLevel = 1;
    private string nameSceneMulti = "LevelMulti";
    private string nameSceneSolo = "Level";

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

    public void loadNextLevel()
    {
        int temp = currentLevel + 1;            
        NetworkManager.singleton.ServerChangeScene(multi ? nameSceneMulti : nameSceneSolo + temp);
        currentLevel++;
    }

    public void LoadCurrentLevel()
    {
        NetworkManager.singleton.ServerChangeScene(multi ? nameSceneMulti : nameSceneSolo + currentLevel);
    }

}
