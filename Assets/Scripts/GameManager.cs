using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Oro")] 
    
    public int oro;
    public TMP_Text oroText;
    
    [Header("Vida")]
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

    public void ReiniciarIA()
    {
        vidaIAMax = vidaIAMax + 10;
        vidaIA = vidaIAMax;
        barraVidaJugador.fillAmount = 1f; 
    }

    public void SumarOro()
    {
        int random = Random.Range(1, 10);

        oro = oro + random; 
        
        oroText.text = oro.ToString();
    }
    
}
