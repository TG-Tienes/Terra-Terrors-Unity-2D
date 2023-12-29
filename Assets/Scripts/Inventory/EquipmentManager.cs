using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.IO;

[DefaultExecutionOrder(-100)]
public class EquipmentManager : MonoBehaviour
{
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

    public Equipment currentWeapon;
    public Equipment[] currentEquipment;
    public Dictionary<EquipType, int> equipTypeDictionary;

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
        LoadEquipmentData();
    }

    public void OnDestroy()
    {
        SaveEquipmentData();
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
        if (newEquipment.equipType == EquipType.WEAPON)
        {
            if (currentEquipment[3] == null)
            {
                AddEquipment(3, newEquipment);
                // Update UI
                onEquipmentChangedCallback?.Invoke();
            }
            else if (currentEquipment[4] == null)
            {
                AddEquipment(4, newEquipment);
                // Update UI
                onEquipmentChangedCallback?.Invoke();
            }
            else
            {
                SwapEquipment(4, newEquipment);
                // Update UI
                onEquipmentChangedCallback?.Invoke();
            }
        }
        else if (currentEquipment[equipTypeDictionary[newEquipment.equipType]] == null)
        {
            // Get slot index
            int slotIndex = equipTypeDictionary[newEquipment.equipType];
            // Add new equipment to the list
            AddEquipment(slotIndex, newEquipment);
            // Update UI
            onEquipmentChangedCallback?.Invoke();
        }
        // If equipment slot is occupied by another equipment
        else
        {
            // Get slot index
            int slotIndex = equipTypeDictionary[newEquipment.equipType];
            // Swap equipment
            SwapEquipment(slotIndex, newEquipment);
            // Update UI
            onEquipmentChangedCallback?.Invoke();
        }
    }

    public void Unequip(int slotIndex, Equipment oldEquipment)
    {
        Inventory.instance.AddItem(oldEquipment);
        currentEquipment[slotIndex] = null;
        StatsManager.instance.UpdateCharacterStatus(null, oldEquipment);
        onEquipmentChangedCallback?.Invoke();
    }

    public void AddEquipment(int slotIndex, Equipment newEquipment)
    {
        // Update player stats
        StatsManager.instance.UpdateCharacterStatus(newEquipment, null);
        // Add new equipment to the list
        currentEquipment[slotIndex] = newEquipment;
    }

    public void SwapEquipment(int slotIndex, Equipment newEquipment)
    {
        // Unequip old equipment of same slot
        Equipment oldEquipment = currentEquipment[slotIndex];
        Unequip(slotIndex, oldEquipment);
        // Update player stats
        StatsManager.instance.UpdateCharacterStatus(newEquipment, null);
        // Add new equipment to the list
        currentEquipment[slotIndex] = newEquipment;
    }

    private const string path = "D:/Unity/GameData/equipmentData.json";

    public void SaveEquipmentData()
    {
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            foreach (Equipment equipment in instance.currentEquipment)
            {
                string json = JsonUtility.ToJson(equipment);
                streamWriter.WriteLine(json);
            }
        }
    }

    public void LoadEquipmentData()
    {
        if (File.Exists(path))
        {
            using (StreamReader streamReader = new StreamReader(path))
            {
                int index = 0;
                while (!streamReader.EndOfStream)
                {
                    // Read a line from the file
                    string json = streamReader.ReadLine();
                    
                    if (!string.IsNullOrEmpty(json))
                    {
                        Equipment equipment = Equipment.LoadEquipmentFromJson(json);
                        if (equipment != null)
                        {
                            instance.Equip(equipment);
                        }
                    }
                    index++;
                }
            }

            Debug.Log("Player Equipment loaded from JSON file.");
        }
        else
        {
            Debug.LogWarning("JSON file not found.");
        }
    }
}