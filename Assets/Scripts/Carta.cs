using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Carta
{
    public TipoCarta tipoCarta;
    public ValorCarta valorCarta;


    public Carta(TipoCarta tipoCarta, ValorCarta valorCarta)
    {
        this.tipoCarta = tipoCarta;
        this.valorCarta = valorCarta;
    }
}

public enum TipoCarta
{
    Corazones, 
    Diamantes,
    Treboles, 
    Picas
}

public enum ValorCarta
{
    Dos,
    Tres,
    Cuatro,
    Cinco, 
    Seis,
    Siete, 
    Ocho,
    Nueve,
    Diez,
    Jota, 
    Reina,
    Rey,
    As
}
