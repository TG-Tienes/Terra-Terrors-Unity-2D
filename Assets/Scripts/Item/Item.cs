using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    CONSUMABLE,
    EQUIPMENT
}

public enum ItemRarity
{
    COMMON,
    UNCOMMON,
    RARE,
    EPIC,
    LEGENDARY,
    MYTHICAL
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item", order = 1)]

public class Item : ScriptableObject
{
    public int ID;
    new public string name;
    public int quantity;
    public Sprite sprite;
    public int spriteID;
    public Sprite bulletSprite;
    public int bulletSpriteID;
    public string description;
    public ItemRarity rarity;
    public ItemType type;

    public int price;
    public virtual void Use() { }

    public virtual void Drop()
    {
        Inventory.instance.RemoveItem(this);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();
        if (player != null)
        {
            Inventory inventory = player.GetComponent<Inventory>();
            inventory.AddItem(this);
        }
    }

    // Load data from JSON
    public static Item LoadFromJson(string jsonString)
    {
        Item data = CreateInstance<Item>();
        JsonUtility.FromJsonOverwrite(jsonString, data);

        if (data.type == ItemType.EQUIPMENT)
        {
            Equipment newData = CreateInstance<Equipment>();
            JsonUtility.FromJsonOverwrite(jsonString, newData);
            return newData;
        }
        if (data.type == ItemType.CONSUMABLE)
        {
            Consumable newData = CreateInstance<Consumable>();
            JsonUtility.FromJsonOverwrite(jsonString, newData);
            return newData;
        }
        return data;
    }
}
