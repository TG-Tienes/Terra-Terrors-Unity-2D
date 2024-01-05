using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    AudioSource _buttonClickedAudio;
    private void Start()
    {
        _buttonClickedAudio = GameObject.Find("SceneAudioManager").gameObject.transform.GetChild(0).GetComponent<AudioSource>();
    }

    private void Update()
    {
        
    }

    public void Resume()
    {
        _buttonClickedAudio.Play();
        pauseMenu.SetActive(false);
        PauseMenuManager.unpauseGame();
    }

    public void ExitLevel()
    {
        _buttonClickedAudio.Play();
        QuestLog.ResetQuestLog();
        QuestManager.instance.Reset();
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(4); 
        PauseMenuManager.unpauseGame();
    }
}
