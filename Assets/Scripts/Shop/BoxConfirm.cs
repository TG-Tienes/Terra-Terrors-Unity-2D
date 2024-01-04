using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

public class BoxConfirm : MonoBehaviour
{
    public int slotIndex;
    public ShopItemType itemType;

    private Item currentItem;

    public TMP_Text confirmText;
    public GameObject blurBG;

    public delegate void OnSelectedItemChangedCallback();
    public OnSelectedItemChangedCallback onSelectedItemChangedCallback;

    #region Singleton
    public static BoxConfirm instance;
    NumberFormatter formatter;

    void Awake()
    {
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
        blurBG.SetActive(false);
        ShopManagerInGame.instance.Purchased(currentItem, currentItem.price, currentItem.ID, itemType.ToString());
    }

    public void Close()
    {
        blurBG.SetActive(false);
        gameObject.SetActive(false);
    }

    public void UpdateItemInfo()
    {
        blurBG.SetActive(true);
        if (slotIndex == 0)
        {
            int idWeapon = PlayerPrefs.GetInt("idWeapon", -1);
            currentItem = ShopManagerInGame.instance.weaponListItem[idWeapon];
            itemType = ShopItemType.WEAPON;
        }
        if (slotIndex == 1)
        {
            int idArmor = PlayerPrefs.GetInt("idArmor", -1);
            currentItem = ShopManagerInGame.instance.armorListItem[idArmor - 2000];
            itemType = ShopItemType.ARMOR;
        }
        if (slotIndex == 2)
        {
            int idConsumable = PlayerPrefs.GetInt("idConsumable", -1);
            currentItem = ShopManagerInGame.instance.consumableListItem[idConsumable - 1000];
            itemType = ShopItemType.CONSUMABLE;
        }

        confirmText.SetText("You want to use " + formatter.FormatNumber(currentItem.price) + "$ to buy this item ?");
    }
}