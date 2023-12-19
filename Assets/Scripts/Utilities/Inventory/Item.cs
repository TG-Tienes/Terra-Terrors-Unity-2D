using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    CONSUMABLE,
    EQUIPMENT
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item", order = 1)]

public class Item : ScriptableObject
{
    new public string name;
    public int quantity;
    public Sprite sprite;
    public string description;
    public ItemType type;

    public virtual void Use() {}
 
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
}