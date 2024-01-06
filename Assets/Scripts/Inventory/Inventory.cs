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
            //DontDestroyOnLoad(this);
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
        // Canvas playerInventory = GameObject.FindGameObjectWithTag("Player Inventory Canvas")?.GetComponent<Canvas>();
        // playerInventory.gameObject.SetActive(true);
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
            Sprite healthPotionSprite = handle_4.Result;
            potionSprites.Add(healthPotionSprite);
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
            Sprite manaPotionSprite = handle_5.Result;
            potionSprites.Add(manaPotionSprite);
        }
        else
        {
            Debug.LogError("Failed to load sprite with addressable key: " + "Mana Potion");
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
                        item.bulletSprite = bulletSprites[item.bulletSpriteID];
                    }
                    else
                    {
                        item.sprite = armorSprites[item.spriteID];
                    }
                    break;
                }
                case ItemType.CONSUMABLE:
                {
                    if (item.ID == 1000)
                    {
                        item.sprite = potionSprites[0];
                    }
                    else if (item.ID == 1001)
                    {
                        item.sprite = potionSprites[1];
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
            items.Add(item);
        }
        SaveInventoryData();
    }
 
    public void AddItem(Item item)
    {
        if (item != null)
        {
            Item existingItem = items.Find(_item => _item.ID == item.ID);
            if (existingItem != null)
            {
                items[items.IndexOf(existingItem)].quantity += 1;
            }
            else
            {
                items.Add(item);
            }
            onItemChangedCallback?.Invoke();
        }
    }
 
    public void RemoveItem(Item item)
    {
        items.Remove(item);
        onItemChangedCallback?.Invoke();
    }

    private static string folderPath = Path.Combine(Application.dataPath, "GameData");
    private static string path = Path.Combine(folderPath, "inventoryData.json");

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
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "");
        }
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
}