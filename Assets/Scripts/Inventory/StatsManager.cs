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
            DontDestroyOnLoad(this);
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
        //SavePlayerData();
    }

    public void UpdateCharacterStatus(Equipment newItem, Equipment oldItem)
    {
        if (oldItem != null)
        {
            playerStats.attack -= oldItem.attackModifier;
            playerStats.defense -= oldItem.defenseModifier;
        }

        if (newItem != null)
        {
            playerStats.attack += newItem.attackModifier;
            playerStats.defense += newItem.defenseModifier;
        }

        onStatusChangedCallback?.Invoke();
    }

    private const string path = "Assets/GameData/playerData.json";

    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(instance.playerStats);
        File.WriteAllText(path, json);
    }

    public static void LoadPlayerData()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            instance.playerStats = PlayerStats.LoadFromJson(json, instance.playerStats);

            Debug.Log("Player Stats loaded from JSON file.");
        }
        else
        {
            Debug.LogWarning("Player data not found. Using default player stats.");
        }
    }
}