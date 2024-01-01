using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
   
    public void Resume()
    {
        pauseMenu.SetActive(false);
        PauseMenuManager.unpauseGame();
    }

    public void ExitLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(6);
    }
}
