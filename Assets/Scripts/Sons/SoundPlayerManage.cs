using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerManage : MonoBehaviour
{
    [SerializeField] private AudioSource weapon;
    [SerializeField] private AudioSource impulse;
    [SerializeField] private AudioSource expulse;
    [SerializeField] private AudioSource deplacement;
    [SerializeField] private AudioSource stop;
    [SerializeField] private AudioSource freeze;

    private bool deplacementIsRun = false;

    private void Start()
    {
        deplacement.volume = 0;
    }

    public void ShotWeaponLeft()
    {
        //weapon.PlayOneShot(weapon.clip);
    }

    public void ShotWeaponRight()
    {
        //weapon.PlayOneShot(weapon.clip);
    }

    public void ShotStop()
    {

    }

    public void ShotFreeze()
    {

    }

    public void ShotImpulse()
    {

    }

    public void ShotExpulse()
    {

    }

    public void StartDeplacement()
    {
        //if (!deplacementIsRun)
        //{
        //    StartCoroutine(AudioController.FadeIn(deplacement, 0.2f));
        //    deplacementIsRun = true;
        //}
    }

    public void StopDeplacement()
    {
        //if (deplacementIsRun)
        //{
        //    StartCoroutine(AudioController.FadeOut(deplacement, 0.5f));
        //    deplacementIsRun = false;
        //}
    }


}
