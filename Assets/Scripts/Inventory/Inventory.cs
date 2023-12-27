using System.Collections.Generic;
using UnityEngine;
 
public enum ItemAction { USE, EQUIP, DROP }

[DefaultExecutionOrder(-100)]
public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int capacity = 20;
    public int stackSize = 5;
 
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
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public void Reset()
    {
        instance.items.Clear();
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
}