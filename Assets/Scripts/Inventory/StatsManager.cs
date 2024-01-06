using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class StatsManager : MonoBehaviour
{
    public delegate void OnStatusChangedCallback();
    public OnStatusChangedCallback onStatusChangedCallback;

    public PlayerStats playerStats;

    #region Singleton
    public static StatsManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public void Start()
    {
        LoadPlayerData();
    }

    public void OnDestroy()
    {
        SavePlayerData();
    }

    public void UpdateCharacterStatus(Equipment newItem, Equipment oldItem)
    {
        if (oldItem != null)
        {
            Debug.Log("- " + oldItem.attackModifier + " : " + oldItem.defenseModifier);
            instance.playerStats.attack -= oldItem.attackModifier;
            instance.playerStats.defense -= oldItem.defenseModifier;
        }

        if (newItem != null)
        {
            Debug.Log("+ " + newItem.attackModifier + " : " + newItem.defenseModifier);
            instance.playerStats.attack += newItem.attackModifier;
            instance.playerStats.defense += newItem.defenseModifier;
        }

        instance.onStatusChangedCallback?.Invoke();
    }

    private static string folderPath = Path.Combine(Application.dataPath, "GameData");
    private static string path = Path.Combine(folderPath, "playerData.json");

    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(instance.playerStats);
        File.WriteAllText(path, json);
    }

    public static void LoadPlayerData()
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "");
        }
        string json = File.ReadAllText(path);
        instance.playerStats = PlayerStats.LoadFromJson(json, instance.playerStats);

        Debug.Log("Player Stats loaded from JSON file.");
    }
}