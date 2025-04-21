using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;
using UnityEngine.UI;

public class ManoManager : MonoBehaviour
{
    public List<Carta> cartasActuales;
    public List<GameObject> cartasActualesGO;
    public Transform holder;
    public GameObject prefabCarta; 
    public DeckManager deckManager;
    public Transform holderMesa; 
    
    void Start()
    {
        RecibirCartasInicio();
    }

    public void RecibirCartasInicio()
    {
        
        List<Carta> cartasRecibidas = deckManager.RepartirCartas(7);
        foreach (Carta carta in cartasRecibidas)
        {
            cartasActuales.Add(carta);
        }

        foreach (Carta carta in cartasActuales)
        {
            GameObject cartaGO = Instantiate(prefabCarta, holder);
            MostrarCarta mostrarCarta = cartaGO.GetComponent<MostrarCarta>();
            mostrarCarta.carta = carta;
            cartaGO.GetComponent<Image>().sprite = carta.sprite;
            cartasActualesGO.Add(cartaGO);
        }
    }

    public void RecibirCartasAlDescartar()
    {
        int cantidadCartas = 7 - cartasActuales.Count;
        List<Carta> cartasARecibir = deckManager.RepartirCartas(cantidadCartas);
        foreach (Carta carta in cartasARecibir)
        {
            cartasActuales.Add(carta);
            GameObject cartaGO = Instantiate(prefabCarta, holder);
            MostrarCarta mostrarCarta = cartaGO.GetComponent<MostrarCarta>();
            mostrarCarta.carta = carta; 
            mostrarCarta.image.sprite = carta.sprite;
            cartasActualesGO.Add(cartaGO);
        }
    }


    public void JugarMano()
    {
        List<Carta> cartasSeleccionadas = cartasActuales.FindAll(c =>c.isSelected);
        CombinacionCartas combinacion = EvaluadorCartas.Evaluar(cartasSeleccionadas);
        if (combinacion == CombinacionCartas.Ninguna)
        {
            Debug.Log("Combinacion no valida");

            foreach (GameObject cartaGo in cartasActualesGO)
            {
                MostrarCarta mostrarCarta = cartaGo.GetComponent<MostrarCarta>();
                if (mostrarCarta.carta.isSelected)
                {
                    mostrarCarta.carta.isSelected = false;
                    mostrarCarta.image.color = Color.white; 
                }
            }
            return; 
        }


        for (int i = 0; i < cartasSeleccionadas.Count; i++)
        {
            int indice = cartasActuales.IndexOf(cartasSeleccionadas[i]);
            GameObject cartaGO = cartasActualesGO[indice];
            cartaGO.transform.SetParent(holderMesa);
            cartaGO.GetComponent<Image>().color=Color.white;
        }
        cartasActuales.RemoveAll(c => c.isSelected);
        cartasActualesGO.RemoveAll(go =>go.GetComponent<MostrarCarta>().carta.isSelected);
    }

   
}
