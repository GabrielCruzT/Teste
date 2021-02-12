using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SomMenu : MonoBehaviour
{
    [Header("Audio")]

    public AudioSource sfxSource;

    public AudioClip sfxMenu;

    void Start()
    {
        sfxSource.clip = sfxMenu;
        sfxSource.volume = 0.15f;
        sfxSource.Play();
    }
}
