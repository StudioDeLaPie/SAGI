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
    private bool multi;

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
            receptacle.activationEvent.AddListener(UpdateWinConditions);
        }
        gameManager = GameManager.instance;
        multi = gameManager.Multi;

        StartCoroutine(OpenEntryCorridor());
        exitCorridor.SetNbPlayers(multi == true ? 2 : 1);
        exitCorridor.allPlayersInsideEvent.AddListener(NextScene);
    }

    private void UpdateWinConditions()
    {
        bool levelPreviouslyFinished = isLevelFinished;
        isLevelFinished = true;
        foreach (ReceptacleCube receptacle in receptaclesCube)
        {
            if (!receptacle.isActivated)
                isLevelFinished = false;
        }

        if (!levelPreviouslyFinished && isLevelFinished)
            Success();
        else if (levelPreviouslyFinished && !isLevelFinished)
            Unsuccess();
    }

    private void Success()
    {
        Debug.Log("Win");
        exitCorridor.OpenDoor();
    }

    private void Unsuccess()
    {
        Debug.Log("Dégagné");
        exitCorridor.CloseDoor();
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
        yield return new WaitForSeconds(2);
        gameManager.playersCoordinatesInCorridor = exitCorridor.GetPlayersLocalCoordinates();
        gameManager.LoadNextLevel();
    }

    private IEnumerator OpenEntryCorridor()
    {
        bool everyoneReady = false;
        while (!everyoneReady)
        {
            Debug.Log("En attente que tout le monde soit ready");
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
            yield return null;
        }
        if (gameManager.playersCoordinatesInCorridor != null)
        {
            entryCorridor.SetPlayersLocalCoordinates(gameManager.playersCoordinatesInCorridor);
        }
        entryCorridor.OpenDoor();
        Debug.Log("Porte ouverte");
    }
}
