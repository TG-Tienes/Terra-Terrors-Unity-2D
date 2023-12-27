using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // start choose world scene
    public void playGame()
    {
        SceneManager.LoadScene("Choose World");
    }

    // close entire app
    public void quitGame()
    {
        Application.Quit();
    }
}
