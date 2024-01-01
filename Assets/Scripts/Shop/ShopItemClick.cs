using System;
using UnityEngine;
using UnityEngine.UI;

public enum ShopItemType
{
    WEAPON,
    ARMOR,
    CONSUMABLE
}


public class ShopItemClick : MonoBehaviour
{
    public Button button;
    int slotIndex;
    public ShopItemType type;
    public void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnItemClick);
    }

    public void OnItemClick()
    {
        string subStr = null;
        int openingParenIndex = button.name.IndexOf('(');
        int closingParenIndex = button.name.IndexOf(')');
        if (openingParenIndex != -1 && closingParenIndex != -1)
            subStr = button.name.Substring(openingParenIndex + 1, closingParenIndex - openingParenIndex - 1);

        if (subStr == null)
            slotIndex = 0;
        else
            slotIndex = int.Parse(subStr);

        ShopManager.instance.itemDetail.SetActive(true);
        ShopManager.instance.blurBG.SetActive(true);

        ItemDetail.instance.slotIndex = slotIndex;
        ItemDetail.instance.itemType = type;

        ItemDetail.instance.onSelectedItemChangedCallback?.Invoke();
    }
}