using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LevelReferee : NetworkBehaviour {

    public List<ReceptacleCube> receptaclesCube = new List<ReceptacleCube>();
    public Corridor entryCorridor;
    public Corridor exitCorridor;

    private bool isLevelFinished = false;
    private GameManager gameManager;

    // Use this for initialization
    void Start () {
		if (receptaclesCube.Count <= 0 || entryCorridor == null || exitCorridor == null)
        {
            Debug.Break();
            Debug.LogError("Le level Referee n'a pas toutes les infos requises.");
        }
        foreach (ReceptacleCube receptacle in receptaclesCube)
        {
            receptacle.activateEvent.AddListener(UpdateWinConditions);
        }
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();


        if (gameManager.playersCoordinatesInCorridor != null)
        {
            bool test = false;
            while (test != true)
            {
                test = entryCorridor.IsReady() && exitCorridor.IsReady();
            }
            entryCorridor.SetPlayersLocalCoordinates(gameManager.playersCoordinatesInCorridor);
        }
        entryCorridor.ExitDoorState(true);
        exitCorridor.bothPlayersInsideEvent.AddListener(PlayersInsideExitCorridor);
        //Debug.Log("Couloir d'entrée ouvert");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void UpdateWinConditions()
    {
        isLevelFinished = true;
        foreach(ReceptacleCube receptacle in receptaclesCube)
        {
            if (!receptacle.activated)
                isLevelFinished = false;
        }

        if (isLevelFinished)
            Win();
    }

    private void Win()
    {
        Debug.Log("Win");
        exitCorridor.EntryDoorState(true);
    }

    private void PlayersInsideExitCorridor()
    {
        exitCorridor.EntryDoorState(false);
        gameManager.playersCoordinatesInCorridor = exitCorridor.GetPlayersLocalCoordinates();
        gameManager.LoadNextLevel();
    }
}
