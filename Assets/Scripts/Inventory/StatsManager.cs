using System.Collections;
using System.Collections.Generic;
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
            Reset();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public void Reset()
    {
        playerStats.mana = playerStats.baseMana;
        playerStats.health = playerStats.baseHealth;
        playerStats.attack = playerStats.baseAttack;
        playerStats.defense = playerStats.baseDefense;
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

        onStatusChangedCallback.Invoke();
    }
}