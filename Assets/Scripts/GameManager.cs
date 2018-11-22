using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private int currentLevel = 1;
    private string nameSceneSolo    = "Level";
    private string nameSceneMulti   = "LevelMulti";

    [SerializeField] private bool multi = false;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void loadNextLevel()
    {
        int temp = currentLevel + 1;
        if(multi)
            SceneManager.LoadScene(nameSceneMulti + temp);
        else
            SceneManager.LoadScene(nameSceneSolo + temp);

        currentLevel++;
    }
}
