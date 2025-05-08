using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum DificultadIA
{
    Facil,
    Normal, 
    Dificil 
}

public class DificultadManager : MonoBehaviour
{
    public DificultadIA dificultad = DificultadIA.Facil;
    public int enemigosDerrotados = 0;
    
    public TMP_Text enemigosDerrotadosText;

    public void RegistrarEnemigoDerrotado()
    {
        enemigosDerrotados++; 
        enemigosDerrotadosText.text = enemigosDerrotados.ToString(); 
        ActualizarDificultad();
        AbrirTienda();
    }

    public void AbrirTienda()
    {
        if (enemigosDerrotados % 5 == 0)  
        {
            GameManager.instance.panelTienda.SetActive(true);
        }
    }

    public void ActualizarDificultad()
    {
        if(enemigosDerrotados <= 5)
        {
            dificultad = DificultadIA.Facil; 
        } else if (enemigosDerrotados < 10)
        {
            dificultad = DificultadIA.Normal;
        }
        else
        {
            dificultad = DificultadIA.Dificil;
        }
        
    }

    public bool DeberiaDescartar(List<Carta> cartas)
    {
        int umbral = dificultad switch
        {
            DificultadIA.Facil => -1,
            DificultadIA.Normal => 6,
            DificultadIA.Dificil => 9,
            _ => -1
        };
        if (umbral < 0) return Random.value > 0.5f;
        int utilidad = CalcularUtilidadCartas(cartas);
        return utilidad < umbral; 
    }

    private int CalcularUtilidadCartas(List<Carta> cartas)
    { 
        List<int> cartasCorazones = new List<int>();
        List<int> cartasPicas = new List<int>();
        List<int> cartasTreboles = new List<int>();
        List<int> cartasDiamantes = new List<int>();
        HashSet<int> valores = new();
        Dictionary<int, int> conteoValores = new(); 
        Dictionary<TipoCarta, int> conteoPalos = new();

        foreach (Carta carta in cartas)
        {
            int valor = EvaluadorCartas.ConvertirValorANumero(carta.valorCarta);
            valores.Add(valor);

            if (!conteoValores.ContainsKey(valor))
            {
                conteoValores[valor] = 0;
            }
            conteoValores[valor]++;

            if (!conteoPalos.ContainsKey(carta.tipoCarta))
            {
                conteoPalos[carta.tipoCarta] = 0;
            }

            conteoPalos[carta.tipoCarta]++;

            if (carta.tipoCarta == TipoCarta.Corazones)
            {
                cartasCorazones.Add(valor);
            } else if (carta.tipoCarta == TipoCarta.Picas)
            {
                cartasPicas.Add(valor);
            } else if (carta.tipoCarta == TipoCarta.Treboles)
            {
                cartasTreboles.Add(valor);
            } else cartasDiamantes.Add(valor);
        }

        int utilidad = 0;
        // Comprobacion Poker, trio, y pareja
        foreach (int valorRepetido in conteoValores.Values)
        {
            
            if (valorRepetido == 4)
            {
                utilidad += 8; 
            } else if (valorRepetido == 3)
            {
                utilidad += 4;
            } else if (valorRepetido == 2)
            {
                utilidad += 1;
            }
        }
        // Comprobacion Doble Pareja
        int numeroParejas = conteoValores.Values.Count(v => v == 2);
        if (numeroParejas >= 2)
        {
            utilidad += 1; 
        }
        
        // Comprobacion Color
        if (conteoPalos.Values.Any(c => c >= 5))
        {
            utilidad += 6; 
        }
        
        bool escaleraBaja = valores.Contains(14) && valores.Contains(2) && valores.Contains(3) && valores.Contains(4) &&
                            valores.Contains(5);
        bool esEscalera = EvaluadorCartas.EsEscalera(valores.ToList()) || escaleraBaja;

        // Comprobacion Escalera
        if (esEscalera)
        {
            utilidad += 5; 
        }
        
        // Comprobacion Full

        bool tieneTrio = conteoValores.Values.Contains(3); 
        bool tienePareja = conteoValores.Values.Contains(2);

        if (tienePareja && tieneTrio) utilidad += 7;
        
        // Comprobacion Escalera Color 
        
        if(EvaluadorCartas.EsEscalera(cartasCorazones) || EvaluadorCartas.EsEscalera(cartasPicas) || EvaluadorCartas.EsEscalera(cartasTreboles) ||  EvaluadorCartas.EsEscalera(cartasDiamantes))
        {
            utilidad += 9; 
        }
        Debug.Log("La utilidad es " + utilidad);
        return utilidad; 
    }

    public int CalcularIntentos()
    {
        return dificultad switch
        {
            DificultadIA.Facil => 10,
            DificultadIA.Normal => 20,
            DificultadIA.Dificil => 100,
            _ => 10
        };
    } 

}
    