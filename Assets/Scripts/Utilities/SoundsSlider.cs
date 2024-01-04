using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundsSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    public GameObject _audioManager;

    void Start()
    {
        _slider.onValueChanged.AddListener((changeVal) =>
        {
            AudioSource[] audioList =  _audioManager.GetComponentsInChildren<AudioSource>();
            foreach(AudioSource audio in audioList)
            {
                audio.volume = changeVal;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
