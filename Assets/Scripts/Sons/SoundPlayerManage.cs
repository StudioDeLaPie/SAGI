using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerManage : MonoBehaviour
{
    [SerializeField] private AudioSource weaponLeft;
    [SerializeField] private AudioSource weaponRight;
    [SerializeField] private AudioSource impulse;
    [SerializeField] private AudioSource expulse;
    [SerializeField] private AudioSource deplacement;
    [SerializeField] private AudioSource stop;
    [SerializeField] private AudioSource freeze;
    [SerializeField] private AudioSource overlay;

    private void Start()
    {
        deplacement.volume = 0;
        overlay.volume = 0;
    }

    public void ShotWeaponLeft()
    {
        weaponLeft.PlayOneShot(weaponLeft.clip);
    }

    public void ShotWeaponRight()
    {
        weaponRight.PlayOneShot(weaponRight.clip);
    }

    public void ShotStop()
    {
        stop.PlayOneShot(stop.clip);
    }

    public void ShotFreeze()
    {
        freeze.PlayOneShot(freeze.clip);
    }

    public void ShotImpulse()
    {
        impulse.PlayOneShot(impulse.clip);
    }

    public void ShotExpulse()
    {
        expulse.PlayOneShot(expulse.clip);
    }

    public void StartDeplacement()
    {
        StartCoroutine(AudioController.FadeIn(deplacement, 0.1f));
    }

    public void StopDeplacement()
    {
        StartCoroutine(AudioController.FadeOut(deplacement, 0.2f));
    }

    public void StartOverlay()
    {
        StartCoroutine(AudioController.FadeIn(overlay, 0.2f));
    }

    public void StopOverlay()
    {
        StartCoroutine(AudioController.FadeOut(overlay, 0.2f));
    }
}
