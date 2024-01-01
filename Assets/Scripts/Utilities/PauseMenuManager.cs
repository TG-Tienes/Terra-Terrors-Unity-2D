using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public static bool _isOpeningMenu = false;

    static void unpauseAudio()
    {
        AudioSource[] allAudioSource = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (var audioSource in allAudioSource)
        {
            audioSource.UnPause();
        }
    }

    static void pauseAudio()
    {
        AudioSource[] allAudioSource = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (var audioSource in allAudioSource)
        {
            audioSource.Pause();
        }
    }

    public static void pauseGame()
    {
        _isOpeningMenu = true;
        pauseAudio();
        Time.timeScale = 0f;
    }

    public static void unpauseGame()
    {
        _isOpeningMenu = false;
        
        unpauseAudio();
        Time.timeScale = 1f;
    }
}
