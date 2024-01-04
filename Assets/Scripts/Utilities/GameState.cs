using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);

            if (pauseMenu.activeSelf)
            {
                PauseMenuManager.pauseGame();

            }
            else
            {
                PauseMenuManager.unpauseGame();
            }
        }
    }
}
