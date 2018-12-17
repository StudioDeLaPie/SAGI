using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

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
        float startLoadingTime = Time.time;
        while (!everyoneReady)
        {
            Debug.Log("En attente que tout le monde soit ready");
            everyoneReady = entryCorridor.IsReady() && exitCorridor.IsReady();

            List<PlayerController> playersController = new List<PlayerController>();
            foreach (ConnectionPlayer player in FindObjectsOfType<ConnectionPlayer>())
            {
                playersController.Add(player.GetComponent<NetworkIdentity>().connectionToClient.playerControllers[0]);
            }
            //Debug.Log("Multi : " + multi);
            //Debug.Log("Nbplayers Lobby : " + LobbyManager.s_Singleton._playerNumber);
            //Debug.Log("Nb controllers récupérés : " + playersController.Count);

            if ((multi && playersController.Count == 2) || (!multi && playersController.Count == 1))
            {
                //    Debug.Log("Bon nombre de joueurs connectés");
                //    for (int i = 0; i < playersController.Count; i++)
                //    {
                //        Debug.Log("Test joueur " + i);
                //        bool estPresentDansLaListe = false;
                //        foreach (NetworkConnection clientReady in LobbyManager.s_Singleton.clientsReady)
                //        {
                //            Debug.Log("Compararaison : clientReady " + clientReady.connectionId + "    Client en test : " + playersController[i].unetView.connectionToClient.connectionId);
                //            if (!estPresentDansLaListe)
                //            {
                //                estPresentDansLaListe = clientReady.connectionId == playersController[i].unetView.connectionToClient.connectionId;
                //                Debug.Log("client " + clientReady.connectionId + " est présent dans la liste des readys : " + estPresentDansLaListe);
                //            }
                //        }
                //        if (!estPresentDansLaListe)
                //            everyoneReady = false;
                //    }
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
            }
            else
            {
                everyoneReady = false;
                Debug.Log("Pas le bon nombre de joueurs");
            }
            yield return null;
        }
        if (gameManager.playersCoordinatesInCorridor != null)
        {
            entryCorridor.SetPlayersLocalCoordinates(gameManager.playersCoordinatesInCorridor);
        }

        while (Time.time < startLoadingTime + 2)
        {
            yield return null; // On attend 2 sec avant d'ouvrir
        }
        entryCorridor.OpenDoor();
        exitCorridor.CloseDoor();
    }
}
