using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAdditivScene : MonoBehaviour {
    [SerializeField] private SceneAsset newScene;
    public GameManager gm;

    void Start ()
    {
        SceneManager.LoadScene(newScene.name, LoadSceneMode.Additive);
        DontDestroyOnLoad(gm.gameObject);
	}
    private void Update()
    {
        if (SceneManager.GetSceneByName(newScene.name).isLoaded)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene.name));
            Destroy(gameObject);
        }
    }
}
