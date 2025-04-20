using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
  public ManoManager manoManager;

  public void Descartar()
  {
    List<Carta> cartas = new List<Carta>();
    foreach (Carta carta in manoManager.cartasActuales)
    {
        if (carta.isSelected)
        {
            cartas.Add(carta);
        }
    }
    manoManager.cartasActuales.RemoveAll(carta=>cartas.Contains(carta));
    
    
    List<GameObject> cartasObj = new List<GameObject>();
    foreach (GameObject cartaGO in manoManager.cartasActualesGO)
    {
        MostrarCarta mostrarCarta = cartaGO.GetComponent<MostrarCarta>();
        foreach (Carta carta in cartas)
        {
            if (mostrarCarta.carta == carta)
            {
                cartasObj.Add(cartaGO);
            }
        }
    }

    foreach (GameObject cartaGO in cartasObj)
    {
        manoManager.cartasActualesGO.Remove(cartaGO);
        Destroy(cartaGO);
    }
   manoManager.RecibirCartasAlDescartar(); 
  }
  
}
