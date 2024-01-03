using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class WeaponSlotController : MonoBehaviour
{
    public List<Image> backgrounds;

    #region Singleton
    public static WeaponSlotController instance;
 
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
    #endregion Singleton

    public void UpdateWeaponSlots(int slotIndex)
    {
        foreach (Image background in backgrounds)
        {
            background.color = GlobalColor.color_weaponDisabled;
        }
        backgrounds[slotIndex].color = GlobalColor.color_weaponEnabled;
    }
}