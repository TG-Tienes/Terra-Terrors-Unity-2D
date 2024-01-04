using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    AudioSource _buttonClicked;

    private void Start()
    {
        _buttonClicked = GameObject.Find("SceneAudioManager").gameObject.transform.GetChild(0).GetComponent<AudioSource>();
    }

    private void Update()
    {
        
    }

    // start choose world scene
    public void playGame()
    {
        Invoke("loadChooseWorldScene", _buttonClicked.clip.length);
        _buttonClicked.Play();
    }

    public void loadChooseWorldScene()
    {
        SceneManager.LoadScene("Choose World");
    }

    // close entire app
    public void quitGame()
    {
        _buttonClicked.Play();
        Application.Quit();
    }

    public void buttonSound()
    {
        _buttonClicked.Play();
    }
}
