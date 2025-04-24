using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidaManager : MonoBehaviour
{
    public float vidaJugador = 100f;

    public float vidaIA = 100f;
    
    float vidaJugadorMax = 100f;
    float vidaIAMax = 100f;
    
    public Image barraVidaJugador;
    public Image barraVidaIA;

    public void RestarVida(bool jugadorGana, float vidaARestar)
    {
        if (jugadorGana)
        {
            vidaIA -= vidaARestar;
        }
        else
        {
            vidaJugador -= vidaARestar;
        }
        ActualizarBarrasVida();
    }

    public void ActualizarBarrasVida()
    {
        barraVidaJugador.fillAmount = vidaJugador / vidaJugadorMax;
        barraVidaIA.fillAmount = vidaIA / vidaIAMax;

    } 
}
