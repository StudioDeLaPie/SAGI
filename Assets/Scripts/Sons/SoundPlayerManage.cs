using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerManage : MonoBehaviour
{
    [SerializeField] private AudioSource impulse;
    [SerializeField] private AudioSource expulse;
    [SerializeField] private AudioSource deplacement;
    [SerializeField] private AudioSource overlay;
    [SerializeField] private AudioClip overlayClip;
    [SerializeField] private AudioClip overlaySuiteClip;

    private void Start()
    {
        deplacement.volume = 0;       
    }

    public void ShotImpulse()
    {
        impulse.PlayOneShot(impulse.clip);
        overlay.Stop();
    }

    public void ShotExpulse()
    {
        expulse.PlayOneShot(expulse.clip);
        overlay.Stop();
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
        if (!overlay.isPlaying)
        {
            overlay.PlayOneShot(overlayClip);

            overlay.Play();

        }
    }

    public void StopOverlay()
    {
        overlay.Stop();
    }
}
