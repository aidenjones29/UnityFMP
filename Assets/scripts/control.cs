using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class control : MonoBehaviour
{
    public GameObject pleaseWait;
    public GameObject pause;
    public GameObject play;
    public GameObject Options;

    public void NextScene()
    {
        pleaseWait.SetActive(true);
        pause.SetActive(false);
        play.SetActive(false);
        Options.SetActive(false);

        SceneManager.LoadScene("fmp");
    }

    public void optionsScene()
    {
        SceneManager.LoadScene("Options");
    }

    public void CloseGame()
    {
        Application.Quit();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
