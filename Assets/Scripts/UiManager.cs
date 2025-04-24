using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    

    public GameObject panelPausa;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void AbrirMenuPausa()
    {
        Time.timeScale = 0; 
        panelPausa.SetActive(true);
    }

    public void CerrarMenuPausa()
    {
        Time.timeScale = 1;
        panelPausa.SetActive(false);
    }

}
