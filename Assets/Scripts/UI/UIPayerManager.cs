using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _UIReticule;
    [SerializeField] private GameObject _UIInfos;
    [SerializeField] private GameObject _UIPauseMenu;

    private bool pauseMenu = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu)
            {
                _UIReticule.SetActive(true);
                _UIPauseMenu.SetActive(false);
                pauseMenu = false;
            }
            else
            {
                _UIReticule.SetActive(false);
                _UIPauseMenu.SetActive(true);
                pauseMenu = true;
            }
        }
    }

    public void ResumeClick()
    {

    }

    public void MainMenuClick()
    {

    }

    public void OptionsClick()
    {

    }

    public void QuitGameClick()
    {

    }
}
