using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class controle_som : MonoBehaviour
{
    [Header("Audio")]

    public AudioSource sfxSource;

    public AudioClip sfxTrem;



    void Start()
    {
        playSFX(sfxTrem, 0.5f);
    }

    public void playSFX(AudioClip sfxClip, float volume)
    {
        sfxSource.PlayOneShot(sfxClip, volume);   
    }

}
