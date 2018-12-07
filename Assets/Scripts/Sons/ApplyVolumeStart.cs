using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Permet d'appliquer les sons définit Au début du jeu
/// </summary>
public class ApplyVolumeStart : MonoBehaviour
{
    [SerializeField] private Slider _ProgressBarMaster;
    [SerializeField] private Slider _ProgressBarMusic;
    [SerializeField] private Slider _ProgressBarSFX;
    [SerializeField] private Slider _ProgressBarUI;

    [SerializeField] private SoundManager _SoundManager;

    void Start()
    {
        _SoundManager.SetVolumeMaster(_ProgressBarMaster.value);
        _SoundManager.SetVolumeMusic(_ProgressBarMusic.value);
        _SoundManager.SetVolumeSFX(_ProgressBarSFX.value);
        _SoundManager.SetVolumeUI(_ProgressBarUI.value);

        Destroy(gameObject);
    }
}
