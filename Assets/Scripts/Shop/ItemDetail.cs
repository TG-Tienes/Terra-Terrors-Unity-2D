using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

public class ItemDetail : MonoBehaviour
{
    public List<Color> colorList = new List<Color>();

    public int slotIndex;
    public ShopItemType itemType;

    private Item currentItem;

    public Image spriteItem;
    public Image backgroundSpriteItem;

    public TMP_Text nameItem;
    public TMP_Text rareItem;
    public TMP_Text attackDataItem;
    public TMP_Text defendDataItem;
    public TMP_Text costItem;

    public delegate void OnSelectedItemChangedCallback();
    public OnSelectedItemChangedCallback onSelectedItemChangedCallback;

    AudioSource _buyItemAudio;

    #region Singleton
    public static ItemDetail instance;
    NumberFormatter formatter;

    void Awake()
    {
        _buyItemAudio = GameObject.Find("SceneAudioManager").gameObject.transform.GetChild(6).GetComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
            instance.onSelectedItemChangedCallback += UpdateItemInfo;
            formatter = new NumberFormatter();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public void HandleBuy()
    {
        _buyItemAudio.Play();  
        ShopManager.instance.Purchased(currentItem, currentItem.price, currentItem.ID, itemType.ToString());
    }

    public void UpdateItemInfo()
    {
        if (itemType.ToString() == "WEAPON")
            currentItem = ShopManager.instance.weaponListItem[slotIndex];
        if (itemType.ToString() == "ARMOR")
            currentItem = ShopManager.instance.armorListItem[slotIndex];
        if (itemType.ToString() == "CONSUMABLE")
            currentItem = ShopManager.instance.consumableListItem[slotIndex];

        spriteItem.enabled = true;
        backgroundSpriteItem.enabled = true;
        nameItem.enabled = true;
        rareItem.enabled = true;
        attackDataItem.enabled = true;
        defendDataItem.enabled = true;
        costItem.enabled = true;

        spriteItem.sprite = currentItem.sprite;
        nameItem.text = currentItem.name;
        rareItem.text = currentItem.rarity.ToString();
        costItem.text = "Cost: " + formatter.FormatNumber(currentItem.price) + "$";

        switch (currentItem.rarity)
        {
            case ItemRarity.COMMON:
                {
                    backgroundSpriteItem.color = Global.ColorCommon;
                    break;
                }
            case ItemRarity.RARE:
                {
                    backgroundSpriteItem.color = Global.ColorRare;
                    break;
                }
            case ItemRarity.EPIC:
                {
                    backgroundSpriteItem.color = Global.ColorEpic;
                    break;
                }
            case ItemRarity.LEGENDARY:
                {
                    backgroundSpriteItem.color = Global.ColorLegendary;
                    break;
                }
            default:
                break;
        }

        if (currentItem.type == ItemType.EQUIPMENT)
        {
            Equipment equipmentItem = (Equipment)currentItem;
            attackDataItem.text = "Attack: +" + equipmentItem.attackModifier;
            defendDataItem.text = "Defense: +" + equipmentItem.defenseModifier;
        }
        else if (currentItem.type == ItemType.CONSUMABLE)
        {

        }
    }
}