using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource stop;
    [SerializeField] private AudioSource freeze;
    [SerializeField] private AudioSource increase;
    [SerializeField] private AudioSource decrease;

    public void ShotStop()
    {
        stop.PlayOneShot(stop.clip);
    }

    public void ShotFreeze()
    {
        freeze.PlayOneShot(freeze.clip);
    }

    public void ShotIncreasse()
    {
        increase.PlayOneShot(increase.clip);
    }

    public void ShotDecreasse()
    {
        decrease.PlayOneShot(decrease.clip);
    }

}
