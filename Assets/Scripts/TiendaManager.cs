using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiendaManager : MonoBehaviour
{

    public List<ObjetoTienda> objetosDisponibles; 
    public List<ObjetoTienda> objetosEnTienda;
    
    public GameManager gameManager;
    public GameObject panelTienda;

    public void ComprarObjeto(ObjetoTienda objeto)
    {
        if (gameManager.oro < objeto.costeObjeto)
        {
            return; 
        } 
        gameManager.oro = gameManager.oro - objeto.costeObjeto;
        gameManager.oroText.text = gameManager.oro.ToString();
        // Efectos Permanantes
        if (objeto.tipoObjeto == TipoObjeto.Permanente)
        {
            gameManager.multiplicadorDano += objeto.aumentoDano;
            gameManager.multiplicadorOroVictoria += objeto.aumentoOroVictoria;
            gameManager.vidaJugadorMax += objeto.aumentoVidaMax;    
        } else if (objeto.tipoObjeto == TipoObjeto.NoPermanente)
        {
            if (objeto.anadirOro != 0)
            {
                objeto.AnadirOro();
            }
            else
            {
                //TODO: Añadir objeto al inventario 
            }
            
        }
        
        
        
    }
}
