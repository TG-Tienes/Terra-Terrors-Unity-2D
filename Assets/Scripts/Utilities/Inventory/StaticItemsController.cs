using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEditor.Build.Content;

[DefaultExecutionOrder(-50)]
public class StaticItemsController : MonoBehaviour
{
    public List<TextMeshProUGUI> fields;
 
    private void Awake()
    {
        StatsManager.instance.onStatusChangedCallback += UpdateFields;
    }

    private void Start()
    {
        UpdateFields();
    }
     
    void UpdateFields()
    {
        Type fieldsType = typeof(PlayerStats);
    
        foreach (TextMeshProUGUI field in fields)
        {
            string value = fieldsType.GetField(field.name).GetValue(StatsManager.instance.playerStats).ToString();
            field.text = value;       
        }
    }
}