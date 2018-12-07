using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private float cooldown = 5;
    private float lastTime;
    [SerializeField] private AudioMixer masterAudioMixer;
    [SerializeField] private AudioMixer musicAudioMixer;
    [SerializeField] private AudioMixer sfxAudioMixer;
    [SerializeField] private AudioSource defaultUISound;
    [SerializeField] private AudioSource connectionUISound;


    private string normal = "Normale", equivalent1 = "Equivalent1", equivalent2 = "Equivalent2";

    void Start()
    {
        lastTime = Time.time;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Time.time > cooldown + lastTime)
        {
            lastTime = Time.time;
            string nextSnapShot;

            //On choisit aléatoirement le prochain snapshot
            switch (Aleatoire.AleatoireBetween(1, 3))
            {
                case 1:
                    nextSnapShot = normal;
                    break;
                case 2:
                    nextSnapShot = equivalent1;
                    break;
                case 3:
                    nextSnapShot = equivalent2;
                    break;
                default:
                    nextSnapShot = normal;
                    break;
            }

            musicAudioMixer.FindSnapshot(nextSnapShot).TransitionTo(3);
        }
    }

    public void ShotDefaultUISound()
    {
        defaultUISound.PlayOneShot(defaultUISound.clip);
    }


   
    public void ShotConnectionUISound()
    {
        connectionUISound.PlayOneShot(connectionUISound.clip);
    }

    #region SetVolume
    
    /// <param name="volume">Compris entre 0 et 1</param>
    public void SetVolumeMaster(float volume)
    {        
        masterAudioMixer.SetFloat("VolumeMaster", volume.Remap(0,1,-80,0));
        //Debug.Log("Master: " + volume.Remap(0, 1, -80, 0));
    }

    /// <param name="volume">Compris entre 0 et 1</param>
    public void SetVolumeMusic(float volume)
    {
        musicAudioMixer.SetFloat("VolumeMusic", volume.Remap(0, 1, -80, 0));
        //Debug.Log("Music : " + volume.Remap(0, 1, -80, 0));
    }

    /// <param name="volume">Compris entre 0 et 1</param>
    public void SetVolumeUI(float volume)
    {
        musicAudioMixer.SetFloat("VolumeUI", volume.Remap(0, 1, -80, 0));
        //Debug.Log("UI : " + volume.Remap(0, 1, -80, 0));
    }

    /// <param name="volume">Compris entre 0 et 1</param>
    public void SetVolumeSFX(float volume)
    {
        sfxAudioMixer.SetFloat("VolumeSFX", volume.Remap(0, 1, -80, 0));
        Debug.Log("SFX : " + volume.Remap(0, 1, -80, 0));
    }
    #endregion

}
