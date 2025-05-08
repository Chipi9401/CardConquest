using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class MostrarObjetoTienda : MonoBehaviour
{
  public ObjetoTienda objetoTienda;

  private void Start()
  {
    Image image = GetComponent<Image>();

    if (objetoTienda.spriteObjeto != null)
    {
      image.sprite = objetoTienda.spriteObjeto;  
    }
    
  }
}
