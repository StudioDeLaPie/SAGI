using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAirControl : MonoBehaviour {

    private PlayerMovementController controller;
    [SerializeField] private RectTransform airControlImage;

	// Use this for initialization
	void Start () {
        controller = GetComponentInParent<PlayerMovementController>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        airControlImage.localScale = new Vector3(1, controller.airControl.currentAC / 100, 1);
	}
}
