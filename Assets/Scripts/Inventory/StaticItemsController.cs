using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[DefaultExecutionOrder(-50)]
public class StaticItemsController : MonoBehaviour
{
    public List<TextMeshProUGUI> fields;

    #region Singleton
    public static StaticItemsController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
 
    private void Start()
    {
        Debug.Log("StaticItemController " + fields.Count);
        StatsManager.instance.onStatusChangedCallback += UpdateFields;
        UpdateFields();
    }
     
    void UpdateFields()
    {
        Type fieldsType = typeof(PlayerStats);
        Debug.Log(fieldsType.ToString());
    
        foreach (TextMeshProUGUI field in fields)
        {
            if (field != null)
            {
                string value = fieldsType.GetField(field.name).GetValue(StatsManager.instance.playerStats).ToString();
                field.text = value;       
            }
        }
    }
}