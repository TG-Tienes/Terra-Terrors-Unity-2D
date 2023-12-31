using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

[DefaultExecutionOrder(-50)]
public class EquipmentSlotController : MonoBehaviour
{
    public int slotIndex;
    public List<Image> spriteFields;
    public List<Image> rarityBackgroundFields;

    public List<Image> weaponSpriteFields;

    public void Awake()
    {
        EquipmentManager.instance.onEquipmentChangedCallback += UpdateAllSlots;
    }

    public void Start()
    {
        UpdateAllSlots();
    }

    public void UpdateWeaponSlot(int slotIndex, Sprite sprite)
    {
        weaponSpriteFields[slotIndex].enabled = true;
        weaponSpriteFields[slotIndex].sprite = sprite;
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
            case ItemRarity.UNCOMMON:
            {
                rarityBackgroundFields[slotIndex].color = GlobalColor.color_Uncommon;
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
            case ItemRarity.MYTHICAL:
            {
                rarityBackgroundFields[slotIndex].color = GlobalColor.color_Mythical;
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
                slotIndex = Array.IndexOf(EquipmentManager.instance.currentEquipment, equipment);
                UpdateSlot(slotIndex, equipment.sprite, equipment.rarity);
                if (equipment.equipType == EquipType.WEAPON)
                {
                    UpdateWeaponSlot(slotIndex - 3, equipment.sprite);
                }
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
        foreach (Image weaponSpriteField in weaponSpriteFields)
        {
            weaponSpriteField.enabled = false;
        }
    }
}