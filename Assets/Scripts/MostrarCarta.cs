using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class MostrarCarta : MonoBehaviour, IPointerClickHandler
{
    public Carta carta;
    public Image image;
    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleSelection();
    }

    public void ToggleSelection()
    {
        carta.isSelected = !carta.isSelected;

        if (carta.isSelected)
        {
            image.color = Color.yellow;
        }
        else
        {
            image.color = Color.white; 
        }
        
    }
    
}
