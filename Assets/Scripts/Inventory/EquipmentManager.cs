using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class EquipmentManager : MonoBehaviour
{
    public Dictionary<EquipType, int> equipTypeDictionary;

    #region Singleton
    public static EquipmentManager instance;
 
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton
 
    public Equipment[] currentEquipment;

    public delegate void OnEquipmentChangedCallback();
    public OnEquipmentChangedCallback onEquipmentChangedCallback;

    public void Start()
    {
        currentEquipment = new Equipment[5];
        equipTypeDictionary = new Dictionary<EquipType, int>
        {
            { EquipType.HEAD, 0 },
            { EquipType.CHEST, 1 },
            { EquipType.LEGS, 2 },
            { EquipType.WEAPON, 3 },
        };
    }

    public void PrintList()
    {
        Debug.Log("List contents: ");
        foreach (Equipment equipment in currentEquipment)
        {
            Debug.Log(equipment);
        }
        Debug.Log("\n");
    }

    public void Equip(Equipment newEquipment)
    {   
        // If equipment slot is currently empty
        if (currentEquipment[equipTypeDictionary[newEquipment.equipType]] == null)
        {
            // Update player stats
            StatsManager.instance.UpdateCharacterStatus(newEquipment, null);
            // Get slot index
            int slotIndex = equipTypeDictionary[newEquipment.equipType];
            // Add new equipment to the list
            currentEquipment[slotIndex] = newEquipment;
            onEquipmentChangedCallback?.Invoke();
        }
        // If equipment slot is occupied by another equipment
        else
        {
            // Get slot index
            int slotIndex = equipTypeDictionary[newEquipment.equipType];
            // Unequip old equipment of same slot
            Equipment oldEquipment = currentEquipment[slotIndex];
            Unequip(oldEquipment);
            // Update player stats
            StatsManager.instance.UpdateCharacterStatus(newEquipment, null);
            // Add new equipment to the list
            currentEquipment[slotIndex] = newEquipment;
            onEquipmentChangedCallback?.Invoke();
        }
    }

    public void Unequip(Equipment oldEquipment)
    {
        Inventory.instance.AddItem(oldEquipment);
        currentEquipment[equipTypeDictionary[oldEquipment.equipType]] = null;
        StatsManager.instance.UpdateCharacterStatus(null, oldEquipment);
        onEquipmentChangedCallback?.Invoke();
    }
}