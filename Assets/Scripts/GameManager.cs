using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Oro")] 
    
    public int oro;
    public TMP_Text oroText;
    
    [Header("Vida")]
    public float vidaJugador = 100f;
    public float vidaIA = 100f;
    
    public float vidaJugadorMax = 100f;
    public float vidaIAMax = 100f;
    
    
    public Image barraVidaJugador;
    public Image barraVidaIA;

    public float multiplicadorDano;
    public float multiplicadorOroVictoria;
    public bool bloqueoDano;

    public List<ObjetoTienda> objetosInventario;
    public Transform panelInventario;

    public GameObject panelTienda; 
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

    public void RestarVida(bool jugadorGana, float vidaARestar)
    {
        if (jugadorGana)
        {
            vidaIA -= vidaARestar;
            AudioManager.instance.ReproducirClip(AudioManager.instance.hacerDano);
        }
        else
        {
            if (bloqueoDano == true)
            {
                return;
            }
            vidaJugador -= vidaARestar;
            AudioManager.instance.ReproducirClip(AudioManager.instance.recibirDano);

        }
        ActualizarBarrasVida();

        bloqueoDano = false;
        
        
    }

    public void ActualizarBarrasVida()
    {
        barraVidaJugador.fillAmount = vidaJugador / vidaJugadorMax;
        barraVidaIA.fillAmount = vidaIA / vidaIAMax;

    }

    public void ReiniciarIA()
    {
        vidaIAMax = vidaIAMax + 10;
        vidaIA = vidaIAMax;
        barraVidaIA.fillAmount = 1f; 
    }

    public void SumarOro()
    {
        int random = Random.Range(1, 10);

        oro = oro + random; 
        
        oroText.text = oro.ToString();
    }
    
}
