using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelReferee : MonoBehaviour {

    public List<ReceptacleCube> receptaclesCube = new List<ReceptacleCube>();
    public Couloir entryCorridor;
    public Couloir exitCorridor;

    private bool isLevelFinished = false;

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
            Debug.Log(receptacle.name + " inscrittion");
        }
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
            Debug.Log("Niveau fini!");
    }
}
