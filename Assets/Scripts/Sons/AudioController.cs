using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioController
{
    private static Dictionary<AudioSource, bool> fadeisRun = new Dictionary<AudioSource, bool>();

    /// <summary>
    /// Test si l'audio source existe dans le dictionnaire
    /// Si il n'existe pas il l'ajoute
    /// </summary>
    /// <param name="audioSource"> AudioSource a tester</param>

    private static void TestDico(AudioSource audioSource)
    {
        if(!fadeisRun.ContainsKey(audioSource))
        {
            fadeisRun.Add(audioSource, false);
        }
    }


    public static IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        TestDico(audioSource);

        if (!fadeisRun[audioSource])
        {
            fadeisRun[audioSource] = true;
            float startVolume = audioSource.volume;
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            fadeisRun[audioSource] = false;
        }
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        TestDico(audioSource);

        if (!fadeisRun[audioSource])
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            //Debug.Log("DebutFadeIn");
            fadeisRun[audioSource] = true;
            while (audioSource.volume < 1)
            {
                audioSource.volume += Time.deltaTime / fadeTime;
                yield return null;
            }
            audioSource.volume = 1;
            fadeisRun[audioSource] = false;
        }
    }
}
