using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemClickInGame : MonoBehaviour
{
    public Button button;
    int slotIndex;

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

        ShopManagerInGame.instance.boxConfirm.SetActive(true);
        ShopManagerInGame.instance.blurBG.SetActive(true);

        BoxConfirm.instance.slotIndex = slotIndex;
        BoxConfirm.instance.onSelectedItemChangedCallback?.Invoke();


    }
}