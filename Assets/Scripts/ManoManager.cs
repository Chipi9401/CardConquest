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
    public Transform holderMesa;
    public Transform holderMesaIA;
    public ManoManager manoManagerIA;
    public VidaManager vidaManager;
    public GameObject botonJugar; 
    public GameObject panelGameOver;

    void Start()
    {
        RecibirCartasInicio();
    }

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

    public void RecibirCartas()
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


    public void JugarMano()
    {
        List<Carta> cartasSeleccionadas = cartasActuales.FindAll(c => c.isSelected);
        CombinacionCartas combinacion = EvaluadorCartas.Evaluar(cartasSeleccionadas);
        if (combinacion == CombinacionCartas.Ninguna)
        {
            Debug.Log("Combinacion no valida");

            foreach (GameObject cartaGo in cartasActualesGO)
            {
                MostrarCarta mostrarCarta = cartaGo.GetComponent<MostrarCarta>();
                if (mostrarCarta.carta.isSelected)
                {
                    mostrarCarta.carta.isSelected = false;
                    mostrarCarta.image.color = Color.white;
                }
            }

            return;
        }


        for (int i = 0; i < cartasSeleccionadas.Count; i++)
        {
            int indice = cartasActuales.IndexOf(cartasSeleccionadas[i]);
            GameObject cartaGO = cartasActualesGO[indice];
            cartaGO.transform.SetParent(holderMesa);
            cartaGO.GetComponent<Image>().color = Color.white;
        }

        cartasActuales.RemoveAll(c => c.isSelected);
        cartasActualesGO.RemoveAll(go => go.GetComponent<MostrarCarta>().carta.isSelected);

        botonJugar.SetActive(false);
        
        
     

        StartCoroutine(RespuestaIA(combinacion)); 
    }

    IEnumerator RespuestaIA(CombinacionCartas combinacionJugador)
    {
        yield return new WaitForSeconds(1.5f);
        //La IA decide si descartar o no
        bool descarta = Random.value > 0.5f;
        if (descarta)
        {
            List<Carta> cartasADescartar = manoManagerIA.cartasActuales.GetRange(0, Random.Range(1, 8));
            List<GameObject> cartasADescartarGO = new List<GameObject>();
            foreach (Carta carta in cartasADescartar)
            {
                int indice = manoManagerIA.cartasActuales.IndexOf(carta);
                cartasADescartarGO.Add(manoManagerIA.cartasActualesGO[indice]);
            }

            foreach (GameObject cartaGO in cartasADescartarGO)
            {
                Destroy(cartaGO);
            }

            manoManagerIA.cartasActuales.RemoveAll(c => cartasADescartar.Contains(c));
            manoManagerIA.cartasActualesGO.RemoveAll(c => cartasADescartarGO.Contains(c));
            
            manoManagerIA.RecibirCartas();
        }

        yield return new WaitForSeconds(1f); 
        //La IA decide que combinacion de cartas jugar

        List<Carta> seleccionCartasIA = ObtenerCombinacionIA(manoManagerIA.cartasActuales);
        CombinacionCartas combinacionIA = EvaluadorCartas.Evaluar(seleccionCartasIA);
        List<GameObject> cartasSeleccionadasIAGO = new List<GameObject>();

        foreach (Carta carta in seleccionCartasIA)
        {
            int indice = manoManagerIA.cartasActuales.IndexOf(carta);
            GameObject cartaGO = manoManagerIA.cartasActualesGO[indice];
            cartaGO.transform.SetParent(holderMesaIA); 
            cartasSeleccionadasIAGO.Add(cartaGO);
        }
        manoManagerIA.cartasActuales.RemoveAll(c =>seleccionCartasIA.Contains(c));
        manoManagerIA.cartasActualesGO.RemoveAll(c => cartasSeleccionadasIAGO.Contains(c));


        yield return new WaitForSeconds(1f);
        //Se decide el resultado del turno
        
        CompararCombinaciones(combinacionJugador, combinacionIA);

        yield return new WaitForSeconds(1f); 
        
       ComprobarVidas();
    }

    public List<Carta> ObtenerCombinacionIA(List<Carta> manoIA)
    {
        List<Carta> mejorCombinacion = null;
        int mejorValorCombinacion = 0;

        //Prueba combinaciones empezando por cinco cartas hasta 1
        for (int i = 5; i >= 1; i--)
        {
            if(manoIA.Count< i) continue;
            //Intenta X combinaciones aleatorias
            for (int intento = 0; intento < 20; intento++)
            {
                List<Carta> manoIACopia = new List<Carta>(manoIA);
                List<Carta> cartasSeleccionadasIA = new List<Carta>();
                //Selecciona las cartas sin repetir
                for (int j = 0; j < i; j++)
                {
                    
                    int indice = Random.Range(0, manoIACopia.Count);
                    cartasSeleccionadasIA.Add(manoIACopia[indice]);
                    manoIACopia.RemoveAt(indice);
                }
                 CombinacionCartas combinacionIA = EvaluadorCartas.Evaluar(cartasSeleccionadasIA);
                 int valorCombinacionIA = EvaluadorCartas.ValorCombinacion(combinacionIA);
                 if (combinacionIA != CombinacionCartas.Ninguna && valorCombinacionIA > mejorValorCombinacion)
                 {
                     mejorCombinacion = cartasSeleccionadasIA;
                     mejorValorCombinacion = valorCombinacionIA; 
                 }
            }

            if (mejorCombinacion != null)
            {
                return mejorCombinacion;
            }
        }
        
        //Si no encuentra ninguna combinacion de cartas de 5, 4 , 3, o 2, va a devolver una carta alta 

        return new List<Carta>
        {
            manoIA[Random.Range(0, manoIA.Count)]
        };
        
    }

    public void CompararCombinaciones(CombinacionCartas combinacionCartasJugador,
        CombinacionCartas combinacionCartasIA)
    {
        int valorCombinacionJugador = EvaluadorCartas.ValorCombinacion(combinacionCartasJugador);
        int valorCombinacionIA = EvaluadorCartas.ValorCombinacion(combinacionCartasIA);

        if (valorCombinacionJugador > valorCombinacionIA)
        {
            Debug.Log("El jugador gana el turno");
            int vidaARestar = valorCombinacionJugador - valorCombinacionIA;
            vidaManager.RestarVida(true, vidaARestar);
        } else if (valorCombinacionJugador < valorCombinacionIA)
        {
            Debug.Log("El jugador pierde el turno");
            int vidaARestar = valorCombinacionIA - valorCombinacionJugador;
            vidaManager.RestarVida(false, vidaARestar);
        }
        else
        {
            Debug.Log("Empate");
        }
        
        
    }

    public void LimpiarMesa()
    {
        for (int i = holderMesa.childCount - 1; i >= 0; i--)
        {
            Destroy(holderMesa.GetChild(i).gameObject);
        }
        for (int i = holderMesaIA.childCount - 1; i >= 0; i--)
        {
            Destroy(holderMesaIA.GetChild(i).gameObject);
        }

    }

    public void ComprobarVidas()
    {
        
        //
        if(vidaManager.vidaJugador <= 0)
        {
           //Si la vida del jugador es menor o igual que cero se activa el panel gameover
           panelGameOver.SetActive(true);
        }else if (vidaManager.vidaIA <= 0)
        {
            //Si la vida del jugador es menor o igual que cero se pasa al siguiente enemigo
            LimpiarListaMano();
        }
        else
        {
            LimpiarMesa();
        
            RecibirCartas();
            manoManagerIA.RecibirCartas();
        }
    }

    [ContextMenu("Limpiar Lista Mano")]
    public void LimpiarListaMano()
    {
        foreach (GameObject obj in cartasActualesGO)
        {
            Destroy(obj);
        }

        foreach (GameObject obj in manoManagerIA.cartasActualesGO)
        {
            Destroy(obj);
        }
        
        cartasActuales.Clear();
        cartasActualesGO.Clear();
        manoManagerIA.cartasActuales.Clear();
        manoManagerIA.cartasActualesGO.Clear(); 
        deckManager.GenerarBaraja();
        RecibirCartasInicio();
        manoManagerIA.RecibirCartasInicio();
    }
} 