using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;  

[DefaultExecutionOrder(-50)]
public class InventorySlotController : MonoBehaviour
{
    public List<Image> spriteFields;
    public List<TextMeshProUGUI> quantityFields;
    public List<Image> rarityBackgroundFields;

    #region Singleton
    public static InventorySlotController instance;
    void Awake()
    {
        if (instance is not null && instance != this)
        {
            Destroy(instance);
            return;
        }
        instance = this;
    }
    #endregion

    // private void Awake()
    // {
    //     DontDestroyOnLoad(this);
    // }

    private void Start()
    {
        Debug.Log(spriteFields.Count + " " + quantityFields.Count + " " + rarityBackgroundFields.Count);
        Inventory.instance.onItemChangedCallback += UpdateAllSlots;
        UpdateAllSlots();
    }

    void SetActiveAndUpdateSlot(int index, Sprite sprite, int quantity, ItemRarity rarity)
    {
        spriteFields[index].enabled = true;
        spriteFields[index].sprite = sprite;

        quantityFields[index].transform.parent.gameObject.SetActive(true);
        quantityFields[index].text = quantity.ToString();

        switch (rarity)
        {
            case ItemRarity.COMMON:
            {
                rarityBackgroundFields[index].color = GlobalColor.color_Common;
                break;
            }
            case ItemRarity.UNCOMMON:
            {
                rarityBackgroundFields[index].color = GlobalColor.color_Uncommon;
                break;
            }
            case ItemRarity.RARE:
            {
                rarityBackgroundFields[index].color = GlobalColor.color_Rare;
                break;
            }
            case ItemRarity.EPIC:
            {
                rarityBackgroundFields[index].color = GlobalColor.color_Epic;
                break;
            }
            case ItemRarity.LEGENDARY:
            {
                rarityBackgroundFields[index].color = GlobalColor.color_Legendary;
                break;
            }
            case ItemRarity.MYTHICAL:
            {
                rarityBackgroundFields[index].color = GlobalColor.color_Mythical;
                break;
            }
            default:
                break;
        };
    }

    void UpdateAllSlots()
    {
        ClearAllSlots();

        int index = 0;
        foreach (Item item in Inventory.instance.items)
        {
            if (item.quantity > Inventory.instance.stackSize)
            {
                for (int iteration = item.quantity / Inventory.instance.stackSize; iteration >= 0; iteration--)
                {
                    if (iteration > 0)
                    {
                        SetActiveAndUpdateSlot(index, item.sprite, Inventory.instance.stackSize, item.rarity);
                    }
                    else
                    {
                        if (item.quantity % Inventory.instance.stackSize != 0)
                        {   
                            SetActiveAndUpdateSlot(index, item.sprite, item.quantity % Inventory.instance.stackSize, item.rarity);
                        }
                    }
                    index++;
                }
            }
            else
            {
                SetActiveAndUpdateSlot(index, item.sprite, item.quantity, item.rarity);
                index++;
            }
        }
    }

    void ClearAllSlots()
    {
        foreach (Image spriteField in spriteFields)
        {
            if (spriteField != null)
            {
                spriteField.enabled = false;
            }
        }
        foreach (TextMeshProUGUI quantityField in quantityFields)
        {
            if (quantityField != null)
                quantityField.transform.parent.gameObject.SetActive(false);
        }
        foreach (Image rarityBackgroundField in rarityBackgroundFields)
        {
            if (rarityBackgroundField != null)
                rarityBackgroundField.color = GlobalColor.color_SlotDefault;
        }
    }
}