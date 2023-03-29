using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBGM : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource audioSource;
    int currentTrack;

    public void PlayMusic()
    {
        if (audioSource.isPlaying)
        {
            return;
        }
        currentTrack--;
        if (currentTrack < 0)
        {
            currentTrack = clips.Length - 1;
        }
        StartCoroutine(WaitForMusicEnd());
    }
    IEnumerator WaitForMusicEnd()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        NextTitle();
    }
    public void NextTitle()
    {
        audioSource.Stop();
        currentTrack++;
        if (currentTrack > clips.Length - 1)
        {
            currentTrack = 0;
        }
        audioSource.clip = clips[currentTrack];
        audioSource.Play();

        StartCoroutine(WaitForMusicEnd());
    }
}
