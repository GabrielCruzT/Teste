using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SomMenu : MonoBehaviour
{ 
    [Header("Audio")]

    public AudioSource sfxOrigem;

    public AudioClip sfxMenu;

    void Start()
    {
        playSFX(sfxMenu, 0.15f);
    }

    public void playSFX(AudioClip sfxClip, float volume)
    {
        //Toca a musica no menu do jogo
        sfxOrigem.PlayOneShot(sfxClip, volume);

    }
}
