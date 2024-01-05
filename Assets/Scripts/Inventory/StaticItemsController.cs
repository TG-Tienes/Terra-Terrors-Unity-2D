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

    private void GetFields()
    {
        fields.Clear();
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("StaticItemText");
        foreach (GameObject obj in taggedObjects)
        {
            TextMeshProUGUI textMeshPro = obj.GetComponent<TextMeshProUGUI>();
            if (textMeshPro != null)
            {
                fields.Add(textMeshPro);
            }
        }
    }
 
    private void Start()
    {
        GetFields();
        StatsManager.instance.onStatusChangedCallback += UpdateFields;
        UpdateFields();
    }

    public void OnDestroy()
    {
        if (StatsManager.instance != null)
        {
            StatsManager.instance.onStatusChangedCallback -= UpdateFields;
        }
    }
     
    void UpdateFields()
    {
        Type fieldsType = typeof(PlayerStats);
    
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