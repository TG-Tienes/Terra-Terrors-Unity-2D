using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum ItemAction { USE, EQUIP, DROP }

[DefaultExecutionOrder(-100)]
public class Inventory : MonoBehaviour
{
    public List<Item> defaultItems = new List<Item>();
    public List<Item> items = new List<Item>();
    public int capacity = 27;
    public int stackSize = 999;
 
    public delegate void OnItemChangedCallback();
    public OnItemChangedCallback onItemChangedCallback;
 
    #region Singleton
    public static Inventory instance;
 
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            Reset();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public void Start()
    {
        //AddTestData();
        LoadInventoryData();
    }

    public void OnDestroy()
    {
        SaveInventoryData();
    }

    public void Reset()
    {
        instance.items.Clear();
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

        foreach (Item item in items)
        {
            switch (item.type)
            {
                case ItemType.EQUIPMENT:
                {
                    Equipment equipment = (Equipment) item;
                    if (equipment.equipType == EquipType.WEAPON)
                    {
                        item.sprite = weaponSprites[item.spriteID];
                    }
                    else
                    {
                        item.sprite = armorSprites[item.spriteID];
                    }
                    break;
                }
                default:
                    break;
            }
        }
    }

    public void AddTestData()
    {
        foreach (Item defaultItem in defaultItems)
        {
            Item item = Instantiate(defaultItem);
            item.quantity = 1;
            items.Add(item);
        }
        SaveInventoryData();
    }
 
    public void AddItem(Item item)
    {
        Item existingItem = items.Find(_item => _item.ID == item.ID);
        if (existingItem != null)
        {
            existingItem.quantity += 1;
        }
        else
        {
            items.Add(item);
        }
        onItemChangedCallback?.Invoke();
    }
 
    public void RemoveItem(Item item)
    {
        items.Remove(item);
        onItemChangedCallback?.Invoke();
    }

    private const string path = "Assets/GameData/inventoryData.json";

    public void SaveInventoryData()
    {
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            foreach (Item item in instance.items)
            {
                string json = JsonUtility.ToJson(item);
                streamWriter.WriteLine(json);
            }
        }
    }

    public void LoadInventoryData()
    {
        if (File.Exists(path))
        {
            // Clear existing data in the list
            instance.items.Clear();

            using (StreamReader streamReader = new StreamReader(path))
            {
                while (!streamReader.EndOfStream)
                {
                    // Read a line from the file
                    string json = streamReader.ReadLine();

                    Item item;
                    item = Item.LoadFromJson(json);
                    instance.items.Add(item);
                }
            }

            LoadSprite();

            Debug.Log("Player Inventory loaded from JSON file.");
        }
        else
        {
            Debug.LogWarning("JSON file not found.");
        }
    }
}