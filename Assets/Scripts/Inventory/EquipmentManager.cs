using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
        Debug.Log("Equipment contains: ");
        foreach (Equipment equipment in currentEquipment)
        {
            if (equipment != null)
            {
                Debug.Log(equipment.name + ", ");
            }
            else
            {
                Debug.Log("null , ");
            }
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
        Inventory.instance.AddItem((Item) oldEquipment);
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

    private const string path = "Assets/GameData/equipmentData.json";

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
            LoadSprite();
            Debug.Log("Player Equipment loaded from JSON file.");
        }
        else
        {
            Debug.LogWarning("JSON file not found.");
        }
    }

    public async void LoadSprite()
    {
        // Load weapon sprites from Addressables
        String weaponAddress = "Weapon";
        Sprite[] weaponSprites_array;
        List<Sprite> weaponSprites = new List<Sprite>();

        AsyncOperationHandle<Sprite[]> handle_1 = Addressables.LoadAssetAsync<Sprite[]>(weaponAddress);
        await handle_1.Task;

        if (handle_1.Status == AsyncOperationStatus.Succeeded)
        {
            weaponSprites_array = handle_1.Result;
            weaponSprites = new List<Sprite>(weaponSprites_array);
        }
        else
        {
            Debug.LogError("Failed to load sprite with addressable key: " + "Weapon");
        }
        
        // Load armor sprites from Addressables
        String armorAddress = "Armor";
        Sprite[] armorSprites_array;
        List<Sprite> armorSprites = new List<Sprite>();

        AsyncOperationHandle<Sprite[]> handle_2 = Addressables.LoadAssetAsync<Sprite[]>(armorAddress);
        await handle_2.Task;

        if (handle_2.Status == AsyncOperationStatus.Succeeded)
        {
            armorSprites_array = handle_2.Result;
            armorSprites = new List<Sprite>(armorSprites_array);
        }
        else
        {
            Debug.LogError("Failed to load sprite with addressable key: " + "Armor");
        }

        foreach (Equipment equipment in currentEquipment)
        {
            if (equipment.equipType == EquipType.WEAPON)
            {
                equipment.sprite = weaponSprites[equipment.spriteID];
            }
            else
            {
                equipment.sprite = armorSprites[equipment.spriteID];
            }
        }
    }
}