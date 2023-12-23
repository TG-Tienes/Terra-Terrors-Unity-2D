using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEditor.Build.Content;
using UnityEngine.UI;

[DefaultExecutionOrder(-50)]
public class InventorySlotController : MonoBehaviour
{
    public List<Image> spriteFields;
    public List<TextMeshProUGUI> quantityFields;

    private void Awake()
    {
        Inventory.instance.onItemChangedCallback += UpdateAllSlots;
    }

    private void Start()
    {
        Debug.Log(Inventory.instance.items.Count.ToString());
        UpdateAllSlots();
    }

    void SetActiveAndUpdateSlot(int index, Sprite sprite, int quantity)
    {
        spriteFields[index].enabled = true;
        spriteFields[index].sprite = sprite;

        quantityFields[index].transform.parent.gameObject.SetActive(true);
        quantityFields[index].text = quantity.ToString();
    }

    void UpdateAllSlots()
    {
        int index = 0;
        foreach (Item item in Inventory.instance.items)
        {
            if (item.quantity > Inventory.instance.stackSize)
            {
                for (int iteration = item.quantity / Inventory.instance.stackSize; iteration >= 0; iteration--)
                {
                    if (iteration > 0)
                    {
                        SetActiveAndUpdateSlot(index, item.sprite, Inventory.instance.stackSize);
                    }
                    else
                    {
                        if (item.quantity % Inventory.instance.stackSize != 0)
                        {   
                            SetActiveAndUpdateSlot(index, item.sprite, item.quantity % Inventory.instance.stackSize);
                        }
                    }
                    index++;
                }
            }
            else
            {
                SetActiveAndUpdateSlot(index, item.sprite, item.quantity);
                index++;
            }
        }
    }

    void ClearAllSlots()
    {
        
    }
}