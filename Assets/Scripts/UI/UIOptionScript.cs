using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOptionScript : MonoBehaviour
{
    [SerializeField] private GameObject _UISoundOptions;
    [SerializeField] private GameObject _UIVideoOptions;
    [SerializeField] private GameObject _UICommandeOptions;
    [SerializeField] private SoundManager _SoundManger;

    private GameObject currentUI;

    private void Start()
    {
        currentUI = _UISoundOptions;
    }

    public void SoundClick()
    {
        _UISoundOptions.SetActive(true);
        currentUI.SetActive(false);
        currentUI = _UISoundOptions;
        _SoundManger.ShotDefaultUISound();
    }

    public void VideoClick()
    {
        _UIVideoOptions.SetActive(true);
        currentUI.SetActive(false);
        currentUI = _UIVideoOptions;
        _SoundManger.ShotDefaultUISound();
    }

    public void CommandeClick()
    {
        _UICommandeOptions.SetActive(true);
        currentUI.SetActive(false);
        currentUI = _UICommandeOptions;
        _SoundManger.ShotDefaultUISound();
    }

    public void ChangeMusicMixer()
    {

    }

    public void ChangeSFXMixer()
    {

    }

}
