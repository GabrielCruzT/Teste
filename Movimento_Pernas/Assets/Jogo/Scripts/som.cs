using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Som : MonoBehaviour
{
    //Som executado ao encostar na bola
    [Header("Audio")]

    public AudioSource sfxSource;

    public AudioClip sfxBola;

    public void playSFX(AudioClip sfxClip, float volume)
    {
        sfxSource.PlayOneShot(sfxClip, volume);
    }

}
