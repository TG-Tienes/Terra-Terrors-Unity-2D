using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;  

[DefaultExecutionOrder(-50)]
public class EquipmentSlotController : MonoBehaviour
{
    public List<Image> spriteFields;
    public List<Image> rarityBackgroundFields;

    public void Start()
    {
        EquipmentManager.instance.onEquipmentChangedCallback += UpdateAllSlots;
    }

    public void UpdateSlot(int slotIndex, Sprite sprite, ItemRarity rarity)
    {
        spriteFields[slotIndex].enabled = true;
        spriteFields[slotIndex].sprite = sprite;

        switch (rarity)
        {
            case ItemRarity.COMMON:
            {
                rarityBackgroundFields[slotIndex].color = GlobalColor.color_Common;
                break;
            }
            case ItemRarity.RARE:
            {
                rarityBackgroundFields[slotIndex].color = GlobalColor.color_Rare;
                break;
            }
            case ItemRarity.EPIC:
            {
                rarityBackgroundFields[slotIndex].color = GlobalColor.color_Epic;
                break;
            }
            case ItemRarity.LEGENDARY:
            {
                rarityBackgroundFields[slotIndex].color = GlobalColor.color_Legendary;
                break;
            }
            default:
                break;
        };
    }

    public void UpdateAllSlots()
    {
        ClearAllSlots();

        foreach (Equipment equipment in EquipmentManager.instance.currentEquipment)
        {
            if (equipment != null)
            {
                UpdateSlot(EquipmentManager.instance.equipTypeDictionary[equipment.equipType], equipment.sprite, equipment.rarity);
            }
        }
    }

    void ClearAllSlots()
    {
        foreach (Image spriteField in spriteFields)
        {
            spriteField.enabled = false;
        }
        foreach (Image rarityBackgroundField in rarityBackgroundFields)
        {
            rarityBackgroundField.color = GlobalColor.color_SlotDefault;
        }
    }
}