using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _UIReticule;
    [SerializeField] private RectTransform airControlImage;
    [SerializeField] private Text txtWeight;

    private PlayerMovementController controller;
    private Weight weight;
    private UIManager _UIManager;

    private bool pauseMenuisActif = false;
    
    private void Start()
    {
        _UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        _UIManager.LocalPlayer = gameObject.GetComponent<UIPlayerManager>();
        controller = GetComponent<PlayerMovementController>();
        weight = GetComponent<Weight>();
    }

    void Update()
    {
        //Echap
        if (Input.GetButtonDown("Cancel"))
        {
            //Si le menu pause n'est pas affiché
            if (!pauseMenuisActif)
            {
                _UIManager.DrawPauseMenu(); //On demande à l'UIManager de l'afficher               
            }
            else
            {
                _UIManager.ResumePauseMenuClick(); //Sinon on demande à l'UImanager de Resume
            }
        }
    }

    void LateUpdate()
    {
        airControlImage.localScale = new Vector3(1, controller.airControl.currentAC / 100, 1);
        txtWeight.text = weight.CurrentWeight.ToString();
    }

    public void SetVisibleHUD(bool visible)
    {
        if(visible)
            _UIReticule.SetActive(true);
        else
            _UIReticule.SetActive(false);
    }

    public bool PauseMenuisActif
    {
        get
        {
            return pauseMenuisActif;
        }

        set
        {
            pauseMenuisActif = value;
        }
    }
}
