using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    int currentLevel = 1;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void loadNextLevel()
    {
        int temp = currentLevel + 1;
        SceneManager.LoadScene("Level" + temp);
        currentLevel++;
    }
}
