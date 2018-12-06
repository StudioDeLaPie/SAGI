using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _UIReticule;    
    private UIManager _UIManager;

    private bool pauseMenuisActif = false;
    
    private void Start()
    {
        _UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        _UIManager.LocalPlayer = gameObject.GetComponent<UIPlayerManager>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (!pauseMenuisActif)
            {
                _UIManager.DrawPauseMenu();
                SetVisibleHUD(false);
                pauseMenuisActif = true;
            }
            else
            {
                _UIManager.ResumePauseMenuClick();
                SetVisibleHUD(true);
                pauseMenuisActif = false;
            }
        }
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
