using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;


public class LoadAdditivScene : MonoBehaviour
{
    [SerializeField] private string newScene;
    public GameManager gm;

    void Start()
    {
        SceneManager.LoadScene(newScene, LoadSceneMode.Additive);
        DontDestroyOnLoad(gm.gameObject);
    }
    private void Update()
    {
        if (SceneManager.GetSceneByName(newScene).isLoaded)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));
            Destroy(gameObject);
        }
    }
}

