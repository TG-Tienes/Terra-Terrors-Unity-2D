using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _slider.onValueChanged.AddListener((changeVal) =>
        {
            _camera.fieldOfView = changeVal;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
