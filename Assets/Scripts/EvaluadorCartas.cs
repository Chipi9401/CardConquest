using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum CombinacionCartas
{
    Ninguna, 
    CartaAlta, 
    Pareja, 
    DoblePareja, 
    Trio, 
    Escalera, 
    Color, 
    Full, 
    Poker, 
    EscaleraColor
}

public static class EvaluadorCartas 
{

    public static CombinacionCartas Evaluar(List<Carta> cartas)
    {
        
        if(cartas == null || cartas.Count == 0) return CombinacionCartas.Ninguna;
        
        int cantidad = cartas.Count; 
        List<int> valores = new List<int>();
        Dictionary<int, int> conteoValores = new(); 
        Dictionary<TipoCarta, int> conteoPalos = new();

        foreach (Carta carta in cartas)
        {
            int valor = ConvertirValorANumero(carta.valorCarta);
            valores.Add(valor);

            if (!conteoValores.ContainsKey(valor))
            {
                conteoValores[valor] = 0;
            }
            conteoValores[valor]++;

            if (!conteoPalos.ContainsKey(carta.tipoCarta))
            {
                conteoPalos[carta.tipoCarta] = 0;
            }
            conteoPalos[carta.tipoCarta]++;
        }
        
        valores.Sort();
        HashSet<int> valoresUnicos = new(valores);
        bool escaleraBaja = valoresUnicos.Contains(14) && valoresUnicos.Contains(2) && valoresUnicos.Contains(3) && valoresUnicos.Contains(4) &&
                            valoresUnicos.Contains(5);
        bool esEscalera = EsEscalera(valoresUnicos.ToList()) || escaleraBaja;
        bool esColor = conteoPalos.ContainsValue(5); 
        
        //Escalera de color

        if (cantidad == 5 && esEscalera && esColor)
        {
            return CombinacionCartas.EscaleraColor;
        }
        
        //Poker

        if (cantidad == 4 && conteoValores.ContainsValue(4))
        {
            return CombinacionCartas.Poker;
        }
        
        //Full

        if (cantidad == 5 && conteoValores.ContainsValue(3) && conteoValores.ContainsValue(2))
        {
            return CombinacionCartas.Full;
        }
        
        //Color

        if (cantidad == 5 && esColor)
        {
            return CombinacionCartas.Color;
        }
        
        //Escalera

        if (cantidad == 5 & esEscalera)
        {
            return CombinacionCartas.Escalera; 
        }
        
        //Trio

        if (cantidad == 3 && conteoValores.ContainsValue(3))
        {
            return CombinacionCartas.Trio; 
        }
        
        //Doble Pareja

        if (cantidad == 4 && conteoValores.Values.Count(v => v == 2) == 2)
        {
            return CombinacionCartas.DoblePareja; 
        }
        
        //Pareja
        
        if (cantidad == 2 && conteoValores.ContainsValue(2))
        {
            return CombinacionCartas.Pareja;
        }
        
        //Carta alta

        if (cantidad == 1)
        {
            return CombinacionCartas.CartaAlta; 
        }
    
        return CombinacionCartas.Ninguna;

    }

    public static bool EsEscalera(List<int> valoresCartas)
    {
        if(valoresCartas.Count < 5) return false; 
        valoresCartas.Sort();
        
        for (int i = 0; i <= valoresCartas.Count - 5; i++)
        {
            bool escalera = true; 
            for (int j = 0; j < 4; j++)
            {
                if (valoresCartas[ i + j + 1 ] - valoresCartas[i+j] != 1 )
                {
                    escalera = false; break;
                }
            }  
            if (escalera)
            {
                return true;
            }
            /*if (valoresCartas[i] == valoresCartas[i + 1])
            {
                escalera = true; 
            }
            else
            {
                escalera = false; break;
            }*/
         
        } return false; 
    }

    public static int ConvertirValorANumero(ValorCarta valor)
    {
        switch (valor)
        {
            case ValorCarta.Dos: return 2;
                break; 
            case ValorCarta.Tres: return 3;
                break; 
            case ValorCarta.Cuatro: return 4;
                break; 
            case ValorCarta.Cinco: return 5;
                break; 
            case ValorCarta.Seis: return 6;
                break; 
            case ValorCarta.Siete: return 7;
                break; 
            case ValorCarta.Ocho: return 8;
                break; 
            case ValorCarta.Nueve: return 9;
                break; 
            case ValorCarta.Diez: return 10;
                break; 
            case ValorCarta.Jota: return 11;
                break; 
            case ValorCarta.Reina: return 12;
                break; 
            case ValorCarta.Rey: return 13;
                break; 
            case ValorCarta.As: return 14;
                break; 
           default: return 0; 
            
        }
    }

    public static int ValorCombinacion(CombinacionCartas combinacion)
    {
        switch (combinacion)
        {
            case CombinacionCartas.Ninguna: return 0;
                break; 
            case CombinacionCartas.CartaAlta: return 1; 
                break;
            case CombinacionCartas.Pareja: return 2;
                break; 
            case CombinacionCartas.DoblePareja: return 3;
                break; 
            case CombinacionCartas.Trio: return 4;
                break; 
            case CombinacionCartas.Escalera: return 5;
                break;
            case CombinacionCartas.Color: return 6;
                break; 
            case CombinacionCartas.Full: return 7;
                break; 
            case CombinacionCartas.Poker: return 8;
                break; 
            case CombinacionCartas.EscaleraColor: return 9;
                break;
            default: return 0; 
        }
    }
    
}
