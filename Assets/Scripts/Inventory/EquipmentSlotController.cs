using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

[DefaultExecutionOrder(50)]
public class EquipmentSlotController : MonoBehaviour
{
    public int slotIndex;
    public List<Image> spriteFields;
    public List<Image> rarityBackgroundFields;
    public List<Image> weaponSpriteFields;

    #region Singleton
    public static EquipmentSlotController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public void Start()
    {
        GetFields();
        EquipmentManager.instance.onEquipmentChangedCallback += UpdateAllSlots;
        UpdateAllSlots();
    }

    public void OnDestroy()
    {
        if (EquipmentManager.instance != null)
        {
            EquipmentManager.instance.onEquipmentChangedCallback -= UpdateAllSlots;
        }
    }

    private void GetFields()
    {
        spriteFields.Clear();
        GameObject[] objects_1 = GameObject.FindGameObjectsWithTag("EquipmentSlotSpriteField");
        List<GameObject> sorted_1 = objects_1.OrderBy(obj => obj.name).ToList();
        foreach (GameObject obj in sorted_1)
        {
            Image result = obj.GetComponent<Image>();
            if (result != null)
            {
                spriteFields.Add(result);
            }
        }

        rarityBackgroundFields.Clear();
        GameObject[] objects_2 = GameObject.FindGameObjectsWithTag("EquipmentSlotRarityBackground");
        List<GameObject> sorted_2 = objects_2.OrderBy(obj => obj.name).ToList();
        foreach (GameObject obj in sorted_2)
        {
            Image result = obj.GetComponent<Image>();
            if (result != null)
            {   
                rarityBackgroundFields.Add(result);
            }
        }

        weaponSpriteFields.Clear();
        GameObject[] objects_3 = GameObject.FindGameObjectsWithTag("WeaponSlotSpriteField");
        List<GameObject> sorted_3 = objects_3.OrderBy(obj => obj.name).ToList();
        foreach (GameObject obj in sorted_3)
        {
            Image result = obj.GetComponent<Image>();
            if (result != null)
            {
                weaponSpriteFields.Add(result);
            }
        }
    }

    public void UpdateWeaponSlot(int slotIndex, Sprite sprite)
    {
        weaponSpriteFields[slotIndex].color = new Color32(255,255,255,255);
        weaponSpriteFields[slotIndex].sprite = sprite;
    }

    public void UpdateSlot(int slotIndex, Sprite sprite, ItemRarity rarity)
    {
        spriteFields[slotIndex].color = new Color32(255,255,255,255);
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

        if (EquipmentManager.instance.currentConsumable != null)
        {
            Consumable consumable = EquipmentManager.instance.currentConsumable;
            UpdateSlot(5, consumable.sprite, consumable.rarity);
            UpdateWeaponSlot(2, consumable.sprite);
        }
    }

    void ClearAllSlots()
    {
        foreach (Image spriteField in spriteFields)
        {
            spriteField.color = new Color32(255,255,255,0);
        }
        foreach (Image rarityBackgroundField in rarityBackgroundFields)
        {
            rarityBackgroundField.color = GlobalColor.color_SlotDefault;
        }
        foreach (Image weaponSpriteField in weaponSpriteFields)
        {
            weaponSpriteField.color = new Color32(255,255,255,0);
        }
    }
}