﻿using System.Collections;
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

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        playersCoordinatesInCorridor = null;
        currentLevel = 1;
    }
}
