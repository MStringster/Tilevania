using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Canvas MainMenuCanvas;
    [SerializeField] Canvas HowToCanvas;

    public void OpenHowToCanvas()
    {
        HowToCanvas.enabled = true;
    }

    public void CloseHowToCanvas()
    {
        HowToCanvas.enabled = false;
    }    


    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
