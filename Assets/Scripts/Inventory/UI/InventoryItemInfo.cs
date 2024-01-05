using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class InventoryItemInfo : MonoBehaviour
{
    public int slotIndex;
    private Item currentItem;

    public Image spriteField;
    public TextMeshProUGUI nameField;
    public TextMeshProUGUI descriptionField;

    public delegate void OnSelectedItemChangedCallback();
    public OnSelectedItemChangedCallback onSelectedItemChangedCallback;

    #region Singleton
    public static InventoryItemInfo instance;
 
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

    public void Start()
    {
        instance.onSelectedItemChangedCallback += UpdateItemInfo;
    }

    public void OnDestroy()
    {
        if (instance != null)
        {
            instance.onSelectedItemChangedCallback -= UpdateItemInfo;
        }
    }

    public void UpdateItemInfo()
    {
        if (slotIndex < Inventory.instance.items.Count)
        {
            currentItem = Inventory.instance.items[slotIndex];

            spriteField.enabled = true;
            nameField.enabled = true;
            descriptionField.enabled = true;

            spriteField.sprite = currentItem.sprite;
            nameField.text = currentItem.name;

            switch (currentItem.rarity)
            {
                case ItemRarity.COMMON:
                {
                    nameField.color = GlobalColor.color_Common;
                    break;
                }
                case ItemRarity.UNCOMMON:
                {
                    nameField.color = GlobalColor.color_Uncommon;
                    break;
                }
                case ItemRarity.RARE:
                {
                    nameField.color = GlobalColor.color_Rare;
                    break;
                }
                case ItemRarity.EPIC:
                {
                    nameField.color = GlobalColor.color_Epic;
                    break;
                }
                case ItemRarity.LEGENDARY:
                {
                    nameField.color = GlobalColor.color_Legendary;
                    break;
                }
                case ItemRarity.MYTHICAL:
                {
                    nameField.color = GlobalColor.color_Mythical;
                    break;
                }
                default:
                    break;
            }

            if (currentItem.type == ItemType.EQUIPMENT)
            {
                Equipment equipmentItem = (Equipment) currentItem;
                descriptionField.text = "Attack: +" + equipmentItem.attackModifier
                                        + "\nDefense: +" + equipmentItem.defenseModifier;
            }
            else if (currentItem.type == ItemType.CONSUMABLE)
            {

            }
        }
        else
        {
            spriteField.enabled = false;
            nameField.enabled = false;
            descriptionField.enabled = false;
        }
    }
}