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


    void Start()
    {
        RecibirCartasInicio();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
