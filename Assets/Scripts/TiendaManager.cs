using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class TiendaManager : MonoBehaviour
{

    public List<ObjetoTienda> objetosDisponiblesNoPermanentes;
    public List<ObjetoTienda> objetosDisponiblesPermanentes; 
    
    public List<ObjetoTienda> objetosEnTienda;
    
   
    public GameObject panelTienda;
    public GameObject panelObjetos;
    public GameObject prefabObjetoTienda;
    
    public GameObject textoInsuficienteOro;
    public float distanciaEnY;
    public float duracionMovimiento;

    public bool textoMoviendose = false;
    
    public IEnumerator TextoInsuficienteOro()
    {
        if (textoMoviendose == true) yield break;
        textoMoviendose = true;
        textoInsuficienteOro.SetActive(true);
        RectTransform rtTexto = textoInsuficienteOro.GetComponent<RectTransform>();
        Vector2 posicionIncialTexto = rtTexto.anchoredPosition;
        Vector2 posicionFinalTexto = posicionIncialTexto + new Vector2(0, distanciaEnY);

        float tiempo = 0;
        while (tiempo < duracionMovimiento)
        {
            rtTexto.anchoredPosition = Vector2.Lerp(posicionIncialTexto, posicionFinalTexto, tiempo/duracionMovimiento);
            tiempo += Time.deltaTime;
            yield return null;
        }
        rtTexto.anchoredPosition = posicionFinalTexto;

        
        textoInsuficienteOro.SetActive(false);
        rtTexto.anchoredPosition = posicionIncialTexto;
        textoMoviendose = false;
    }
    
    public void Start()
    {
         InstanciarObjetosTienda();
    }

    public void SeleccionarObjetosTienda()
    {
        objetosEnTienda.Clear();
        if (objetosDisponiblesNoPermanentes.Count >= 2)
        {
            List<ObjetoTienda> objetosTienda = objetosDisponiblesNoPermanentes.OrderBy(o =>UnityEngine.Random.value).Take(2).ToList();;
            objetosEnTienda.AddRange(objetosTienda);
        }
        
        if (objetosDisponiblesPermanentes.Count >= 2)
        {
            List<ObjetoTienda> objetosTienda = objetosDisponiblesPermanentes.OrderBy(o =>UnityEngine.Random.value).Take(2).ToList();;
            objetosEnTienda.AddRange(objetosTienda);
        }
    }

    public void InstanciarObjetosTienda()
    {
        foreach (Transform t in panelObjetos.transform)
        {
            Destroy(t.gameObject);
        }
        SeleccionarObjetosTienda();
        foreach (ObjetoTienda objeto in objetosEnTienda)
        {
            GameObject obj = Instantiate(prefabObjetoTienda, panelObjetos.transform);
            MostrarObjetoTienda mostrarObjetoTienda = obj.GetComponent<MostrarObjetoTienda>();
            mostrarObjetoTienda.objetoTienda = objeto;
            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(() => ComprarObjeto(objeto,obj));
        }
    }
    
    public void ComprarObjeto(ObjetoTienda objeto, GameObject obj)
    {
        if (GameManager.instance.oro < objeto.costeObjeto)
        {
            StartCoroutine(TextoInsuficienteOro());
            return; 
        } 
        AudioManager.instance.ReproducirClip(AudioManager.instance.comprarObjeto);
        GameManager.instance.oro = GameManager.instance.oro - objeto.costeObjeto;
        GameManager.instance.oroText.text = GameManager.instance.oro.ToString();
        // Efectos Permanantes
        if (objeto.tipoObjeto == TipoObjeto.Permanente)
        {
            GameManager.instance.multiplicadorDano += objeto.aumentoDano; 
            GameManager.instance.multiplicadorOroVictoria += objeto.aumentoOroVictoria;
            GameManager.instance.vidaJugadorMax += objeto.aumentoVidaMax;    
        } else if (objeto.tipoObjeto == TipoObjeto.NoPermanente)
        {
            if (objeto.anadirOro != 0)
            {
                objeto.AnadirOro();
            }
            else
            {

                GameManager.instance.objetosInventario.Add(objeto);
                GameObject objetoInventario = Instantiate(prefabObjetoTienda, GameManager.instance.panelInventario);
                MostrarObjetoTienda mostrarObjetoTienda = objetoInventario.GetComponent<MostrarObjetoTienda>();
                mostrarObjetoTienda.objetoTienda = objeto;
                Button button = objetoInventario.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                if(mostrarObjetoTienda.objetoTienda.nombreObjeto == "Bloqueo del Daño")
                {
                    button.onClick.AddListener(() => mostrarObjetoTienda.objetoTienda.BloquearDano());
                    Debug.Log("Bloquear Daño");

                } else if (mostrarObjetoTienda.objetoTienda.nombreObjeto == "Daño Instantáneo")
                {
                    button.onClick.AddListener(() => mostrarObjetoTienda.objetoTienda.AplicarDano());
                    Debug.Log("Aplicar Daño");

                }
                button.onClick.AddListener(() => AudioManager.instance.ReproducirClip(AudioManager.instance.usarObjeto));
                button.onClick.AddListener(() => Destroy(objetoInventario));
                
            }
            
        }
        
        Destroy(obj);
        
        
    }
}
