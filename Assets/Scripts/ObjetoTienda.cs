using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoObjeto
{
    Permanente, 
    NoPermanente
} 
[System.Serializable]
public class ObjetoTienda 
{
  public string nombreObjeto;
  public int costeObjeto; 
  public string descripcionObjeto;
  public TipoObjeto tipoObjeto;
  public Sprite spriteObjeto;

  public bool haSidoComprado = false; 


  [Header("Efectos Permanantes")] 
  public int aumentoDano;

  public int aumentoOroVictoria;

  public int aumentoVidaMax;
  [Header("Efectos No Permanentes")] 
  public int danoInstantaneo;

  public bool bloquearDano;

  public int anadirOro;

  
  public void AplicarDano()
  {
      GameManager.instance.vidaIA -= danoInstantaneo; 
      Debug.Log("Daño instantaneo");
  }

  public void BloquearDano()
  {
      GameManager.instance.bloqueoDano = true;
      Debug.Log("Bloquear daño");
  }

  public void AnadirOro()
  {
      GameManager.instance.oro += anadirOro; 
      Debug.Log("Añadir oro");
  }
  
}
