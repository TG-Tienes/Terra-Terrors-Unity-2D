using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyAudio : MonoBehaviour
{
    //private static DontDestroyAudio _instance;

    //private void Awake()
    //{
    //    //if we don't have an [_instance] set yet
    //    if (!_instance)
    //        _instance = this;
    //    //otherwise, if we do, kill this thing
    //    else
    //        Destroy(this.gameObject);

    //    DontDestroyOnLoad(this.gameObject);
    //}

    public static int menuScreenBuildIndex; //the menu screen's index in your Build Settings
    private static DontDestroyAudio _instance;

    void Awake()
    {
        if (!_instance)
            _instance = this;
        //otherwise, if we do, kill this thing
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(transform.root.gameObject);

        SceneManager.activeSceneChanged += DestroyOnMenuScreen;
    }

    void DestroyOnMenuScreen(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex == 1 || newScene.buildIndex == 2 || newScene.buildIndex == 3) //could compare Scene.name instead
        {
            this.gameObject.transform.GetChild(0).GetComponent<AudioSource>().Stop();
            this.gameObject.transform.GetChild(1).GetComponent<AudioSource>().Play();
        }

        if (newScene.buildIndex == 0 || newScene.buildIndex == 4)
        {
            if (this.gameObject.transform.GetChild(1).GetComponent<AudioSource>() != null && this.gameObject.transform.GetChild(1).GetComponent<AudioSource>().isPlaying)
            {
                this.gameObject.transform.GetChild(0).GetComponent<AudioSource>().Play();
                this.gameObject.transform.GetChild(1).GetComponent<AudioSource>().Stop();
            }
        }
    }
}
