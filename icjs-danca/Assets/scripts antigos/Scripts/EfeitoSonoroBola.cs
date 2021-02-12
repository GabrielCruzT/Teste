using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EfeitoSonoroBola : MonoBehaviour
{
    [Header("Audio")]

    public AudioSource sfxSource;

    public AudioClip sfxBola;

    public void playSFX(AudioClip sfxClip, float volume)
    {
        sfxSource.PlayOneShot(sfxClip, volume);
    }

}
