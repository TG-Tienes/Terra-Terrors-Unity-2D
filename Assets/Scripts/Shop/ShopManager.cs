using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public int coin;
    private TMP_Text coinUI;
    public Equipment[] armorListItem;
    public Equipment[] weaponListItem;
    public Consumable[] consumableListItem;

    public GameObject[] shopArmorPanelsItem;
    public GameObject[] shopWeaponPanelsItem;
    public GameObject[] shopConsumablePanelsItem;

    public ShopTemplate[] shopArmorPanels;
    public ShopTemplate[] shopWeaponPanels;
    public ShopTemplate[] shopConsumablePanels;


    public static ShopManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        coinUI = GameObject.Find("CoinPlayer").GetComponent<TMP_Text>();
        coinUI.text = coin.ToString();


        for (int i = 0; i < weaponListItem.Length; i++)
            shopWeaponPanelsItem[i].SetActive(true);

        LoadPanels("weapon");
    }

    public void LoadPanels(string typePanel)
    {
        if (typePanel == "weapon")
        {
            for (int i = 0; i < weaponListItem.Length; i++)
            {
                shopWeaponPanelsItem[i].SetActive(true);

                shopWeaponPanels[i].nameItem.text = weaponListItem[i].name;
                shopWeaponPanels[i].imgItem.sprite = weaponListItem[i].sprite;
                shopWeaponPanels[i].priceItem.text = weaponListItem[i].price.ToString();
            }
        }

        if (typePanel == "armor")
        {
            for (int i = 0; i < armorListItem.Length; i++)
            {
                shopArmorPanelsItem[i].SetActive(true);

                shopArmorPanels[i].nameItem.text = armorListItem[i].name;
                shopArmorPanels[i].imgItem.sprite = armorListItem[i].sprite;
                shopArmorPanels[i].priceItem.text = armorListItem[i].price.ToString();
            }
        }

        if (typePanel == "consumable")
        {
            for (int i = 0; i < consumableListItem.Length; i++)
            {
                shopConsumablePanelsItem[i].SetActive(true);

                shopConsumablePanels[i].nameItem.text = consumableListItem[i].name;
                shopConsumablePanels[i].imgItem.sprite = consumableListItem[i].sprite;
                shopConsumablePanels[i].priceItem.text = consumableListItem[i].price.ToString();
            }
        }
    }
}