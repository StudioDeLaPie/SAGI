using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LevelReferee : NetworkBehaviour
{

    public List<ReceptacleCube> receptaclesCube = new List<ReceptacleCube>();
    public Corridor entryCorridor;
    public Corridor exitCorridor;

    private bool isLevelFinished = false;
    private GameManager gameManager;

    // Use this for initialization
    private void Start()
    {
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

        StartCoroutine(OpenEntryCorridor());
        exitCorridor.bothPlayersInsideEvent.AddListener(NextScene);
    }

    private void UpdateWinConditions()
    {
        isLevelFinished = true;
        foreach (ReceptacleCube receptacle in receptaclesCube)
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
        exitCorridor.OpenDoor();
    }

    /// <summary>
    /// Appelé lors de l'évènement "les 2 joueurs sont dans le tunnel de sortie"
    /// </summary>
    private void NextScene()
    {
        if (isLevelFinished)
            StartCoroutine(NextSceneCoroutine());
    }

    private IEnumerator NextSceneCoroutine()
    {
        exitCorridor.CloseDoor();
        yield return new WaitForSeconds(3);
        gameManager.playersCoordinatesInCorridor = exitCorridor.GetPlayersLocalCoordinates();
        gameManager.LoadNextLevel();
    }

    private IEnumerator OpenEntryCorridor()
    {
        bool everyoneReady = false;
        while (!everyoneReady)
        {
            everyoneReady = entryCorridor.IsReady() && exitCorridor.IsReady();
            if (gameManager.playersCoordinatesInCorridor != null)
            {
                foreach (Corridor.PlayerPositionInCorridor player in gameManager.playersCoordinatesInCorridor)
                {
                    if (everyoneReady)
                    {
                        everyoneReady = player.playerConnection.isReady;
                    }
                }
            }
            Debug.Log("everyoneReady = " + everyoneReady + "  " + Time.time);
            yield return null;
        }
        if (gameManager.playersCoordinatesInCorridor != null)
        {
            Debug.Log("Placement des joueurs");
            entryCorridor.SetPlayersLocalCoordinates(gameManager.playersCoordinatesInCorridor);
        }
        Debug.Log("Ouverture de la porte : " + Time.time);
        entryCorridor.OpenDoor();
    }
}
