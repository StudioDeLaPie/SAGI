using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Variables : MonoBehaviour {

    public bool tutorielMode;    
    public SceneAsset sceneTuto;
    public SceneAsset sceneMulti;
    public NetworkLobbyPlayer playerInfosSolo;
    public NetworkLobbyPlayer playerInfosMulti;
}
