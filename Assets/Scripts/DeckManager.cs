using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckManager : MonoBehaviour
{
    public List<Carta> deck = new List<Carta>();

    private void Awake()
    {
        GenerarBaraja(); 
    } 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public List<Carta>RepartirCartas(int cantidad)
    {
        List<Carta> cartas = new List<Carta>(); 
        for (int i = 0; i < cantidad; i++)
        {
            cartas.Add(deck[0]);
            deck.RemoveAt(0);
            
        } return cartas;
    }

    void BarajarDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Carta temp = deck[i];
            int random = Random.Range(i, deck.Count);
            deck[i] = deck[random];
            deck[random] = temp;
        }
    }

    void GenerarBaraja()
    {
        deck.Clear();
        foreach (TipoCarta tipo in Enum.GetValues(typeof(TipoCarta)))
        {
            foreach (ValorCarta valor in Enum.GetValues(typeof(ValorCarta)))
            {
                deck.Add(new Carta(tipo, valor));
            }
        }
        BarajarDeck();
    }
    
}
