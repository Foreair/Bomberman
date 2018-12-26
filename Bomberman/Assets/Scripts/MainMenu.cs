using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayGameSinglePlayer()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("1P Level1");
    }

    public void PlayGameMultiPlayer()
    {
        SceneManager.LoadScene("2P Level1");
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

}
