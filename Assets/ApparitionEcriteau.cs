using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApparitionEcriteau : MonoBehaviour {

    private Vector3 startScale;

	// Use this for initialization
	void Start () {
        startScale = transform.localScale;
        transform.localScale = Vector3.zero;
        FindObjectOfType<ReceptacleCube>().activationEvent.AddListener(Apparition);
	}
	
	private void Apparition()
    {
        StartCoroutine(ApparitionCoroutine());
    }

    private IEnumerator ApparitionCoroutine()
    {
        Vector3 newScale = startScale / 10;
        while (newScale.x < startScale.x)
        {
            newScale *= 1.1f;
            transform.localScale = newScale;
            yield return null;
        }
    }
}
