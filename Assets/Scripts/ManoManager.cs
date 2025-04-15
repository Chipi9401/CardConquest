using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;

public class ManoManager : MonoBehaviour
{
    public List<Carta> cartasActuales;
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
            Instantiate(prefabCarta, holder);
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
