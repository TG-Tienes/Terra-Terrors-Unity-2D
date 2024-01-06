using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[DefaultExecutionOrder(-75)]
public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    public Consumable currentConsumable;
    public Equipment currentWeapon;
    public Equipment[] currentEquipment;
    public Dictionary<EquipType, int> equipTypeDictionary;

    public delegate void OnEquipmentChangedCallback();
    public OnEquipmentChangedCallback onEquipmentChangedCallback;

    AudioSource _equipAudio;

    public void Start()
    {
        GameObject sceneAudioManager = GameObject.Find("SceneAudioManager").gameObject;

        _equipAudio = sceneAudioManager.transform.GetChild(1).GetComponent<AudioSource>();

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
        ResetStats();
        SaveEquipmentData();
    }

    public void ResetStats()
    {
        foreach (Equipment equipment in instance.currentEquipment)
        {
            if (equipment != null)
            {
                StatsManager.instance.playerStats.attack -= equipment.attackModifier;
                StatsManager.instance.playerStats.defense -= equipment.defenseModifier;
            }
        }
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
        //_equipAudio.Play();

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
        Inventory.instance.AddItem((Item)oldEquipment);
        currentEquipment[slotIndex] = null;
        StatsManager.instance.UpdateCharacterStatus(null, oldEquipment);
        onEquipmentChangedCallback?.Invoke();
    }

    public void AddEquipment(int slotIndex, Equipment newEquipment)
    {
        // Update player stats
        StatsManager.instance.UpdateCharacterStatus(newEquipment, null);
        Debug.Log("Attack= " + StatsManager.instance.playerStats.attack + " ; Defense= " + StatsManager.instance.playerStats.defense); 
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

    public void EquipConsumable(Consumable newConsumable)
    {
        if (currentConsumable != null)
        {
            Inventory.instance.items.Add((Item) currentConsumable);
        }
        currentConsumable = newConsumable;
        onEquipmentChangedCallback?.Invoke();
    }

    private static string folderPath = Path.Combine(Application.dataPath, "GameData");
    private static string path = Path.Combine(folderPath, "equipmentData.json");

    public void SaveEquipmentData()
    {
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            foreach (Equipment equipment in instance.currentEquipment)
            {
                string json = JsonUtility.ToJson(equipment);
                streamWriter.WriteLine(json);
            }

            //StatsManager.instance.SavePlayerData();
            if (currentConsumable != null)
            {
                string json = JsonUtility.ToJson(currentConsumable);
                streamWriter.WriteLine(json);
            }
        }
    }

    public void LoadEquipmentData()
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "");
        }
        using (StreamReader streamReader = new StreamReader(path))
        {
            int index = 0;
            while (!streamReader.EndOfStream)
            {
                // Read a line from the file
                string json = streamReader.ReadLine();

                if (!string.IsNullOrEmpty(json))
                {
                    Item item = Item.LoadFromJson(json);
                    if (item != null)
                    {
                        switch (item.type)
                        {
                            case ItemType.EQUIPMENT:
                            {
                                instance.Equip((Equipment) item);
                                break;
                            }
                            case ItemType.CONSUMABLE:
                            {
                                instance.EquipConsumable((Consumable) item);
                                break;
                            }
                            default:
                                break;
                        }
                    }
                }
                index++;
            }
        }

        LoadSprite();

        Debug.Log("Player Equipment loaded from JSON file.");
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

        // Load bullet sprites from Addressables
        String bulletAddress = "Bullet";
        Sprite[] bulletSprites_array;
        List<Sprite> bulletSprites = new List<Sprite>();

        AsyncOperationHandle<Sprite[]> handle_3 = Addressables.LoadAssetAsync<Sprite[]>(bulletAddress);
        await handle_3.Task;

        if (handle_3.Status == AsyncOperationStatus.Succeeded)
        {
            bulletSprites_array = handle_3.Result;
            bulletSprites = new List<Sprite>(bulletSprites_array);
        }
        else
        {
            Debug.LogError("Failed to load sprite with addressable key: " + "Bullet");
        }

        // Load health potion sprite from Addressables
        String healthPotionAddress = "Health Potion";
        List<Sprite> potionSprites = new List<Sprite>();

        AsyncOperationHandle<Sprite> handle_4 = Addressables.LoadAssetAsync<Sprite>(healthPotionAddress);
        await handle_4.Task;

        if (handle_4.Status == AsyncOperationStatus.Succeeded)
        {
            potionSprites.Add(handle_4.Result);
        }
        else
        {
            Debug.LogError("Failed to load sprite with addressable key: " + "Health Potion");
        }

        // Load health potion sprite from Addressables
        String manaPotionAddress = "Mana Potion";

        AsyncOperationHandle<Sprite> handle_5 = Addressables.LoadAssetAsync<Sprite>(manaPotionAddress);
        await handle_5.Task;

        if (handle_5.Status == AsyncOperationStatus.Succeeded)
        {
            potionSprites.Add(handle_5.Result);
        }
        else
        {
            Debug.LogError("Failed to load sprite with addressable key: " + "Mana Potion");
        }

        foreach (Equipment equipment in currentEquipment)
        {
            if (equipment != null)
            {
                if (equipment.equipType == EquipType.WEAPON)
                {
                    equipment.sprite = weaponSprites[equipment.spriteID];
                    equipment.bulletSprite = bulletSprites[equipment.bulletSpriteID];
                }
                else
                {
                    equipment.sprite = armorSprites[equipment.spriteID];
                }
            }
        }
        if (currentConsumable != null)
        {
            switch (currentConsumable.name)
            {
                case "Health Potion":
                    currentConsumable.sprite = potionSprites[0];
                    break;
                case "Mana Potion":
                    currentConsumable.sprite = potionSprites[1];
                    break;
                default:
                    break;
            };
        }
    }
}