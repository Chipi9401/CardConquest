using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioSource audioSource;

    public AudioClip seleccionarCarta;
    public AudioClip hacerDano;
    public AudioClip recibirDano;
    public AudioClip eliminarOponente;
    public AudioClip gameOver;
    public AudioClip usarObjeto;
    public AudioClip comprarObjeto;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ReproducirClip(AudioClip clip)
    { 
        audioSource.clip = clip;
        audioSource.Play();
    }

}
