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
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
 
    #endregion
 
    public void UpdateCharacterStatus(Equipment newItem, Equipment oldItem)
    {
        if(oldItem != null)
        {
            playerStats.attack -= oldItem.attackModifier;
            playerStats.defense -= oldItem.defenseModifier;
        } 
            
        playerStats.attack = playerStats.baseAttack + newItem.attackModifier;
        playerStats.defense = playerStats.baseDefense + newItem.defenseModifier;

        onStatusChangedCallback.Invoke();
    }
}