using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateHealthBar(float current, float maxVal)
    {
        _slider.value = current / maxVal;
    }
}
