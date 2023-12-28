using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-200)]
public class ShopManager : MonoBehaviour
{
    Color colorRare(string rare)
    {
        Color color = Color.black;

        if (rare == "COMMON")
            return Global.ColorCommon;

        if (rare == "RARE")
            return Global.ColorRare;

        if (rare == "EPIC")
            return Global.ColorEpic;

        if (rare == "LEGENDARY")
            return Global.ColorLegendary;

        return color;
    }

    public int coin;
    private TMP_Text coinUI;
    public List<Equipment> armorListItem;
    public List<Equipment> weaponListItem;
    public List<Consumable> consumableListItem;

    public List<GameObject> shopArmorPanelsItem;
    public List<GameObject> shopWeaponPanelsItem;
    public List<GameObject> shopConsumablePanelsItem;

    public List<ShopTemplate> shopArmorPanels;
    public List<ShopTemplate> shopWeaponPanels;
    public List<ShopTemplate> shopConsumablePanels;
    public GameObject itemDetail;
    public GameObject blurBG;

    public float displayTime = 2f;
    public GameObject notEnoughCoinBox;

    #region Singleton
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

    #endregion Singleton

    private void Start()
    {
        coinUI = GameObject.Find("CoinPlayer").GetComponent<TMP_Text>();
        coinUI.text = coin.ToString();

        for (int i = 0; i < weaponListItem.Count; i++)
            shopWeaponPanelsItem[i].SetActive(true);

        LoadPanels("weapon");
    }

    public void LoadPanels(string typePanel)
    {
        if (typePanel == "weapon")
        {
            for (int i = 0; i < weaponListItem.Count; i++)
            {
                if (weaponListItem[i].quantity != 0)
                {
                    weaponListItem[i].ID = i;
                    shopWeaponPanelsItem[i].SetActive(true);

                    shopWeaponPanels[i].rareItem.color = colorRare(weaponListItem[i].rarity.ToString());
                    shopWeaponPanels[i].nameItem.text = weaponListItem[i].name;
                    shopWeaponPanels[i].imgItem.sprite = weaponListItem[i].sprite;
                    shopWeaponPanels[i].priceItem.text = weaponListItem[i].price.ToString();
                }
                else
                {
                    shopWeaponPanelsItem[i].SetActive(false);
                }
            }
        }

        if (typePanel == "armor")
        {
            for (int i = 0; i < armorListItem.Count; i++)
            {
                if (armorListItem[i].quantity != 0)
                {
                    armorListItem[i].ID = i;
                    shopArmorPanelsItem[i].SetActive(true);

                    shopArmorPanels[i].rareItem.color = colorRare(armorListItem[i].rarity.ToString());
                    shopArmorPanels[i].nameItem.text = armorListItem[i].name;
                    shopArmorPanels[i].imgItem.sprite = armorListItem[i].sprite;
                    shopArmorPanels[i].priceItem.text = armorListItem[i].price.ToString();
                }
                else
                {
                    shopArmorPanelsItem[i].SetActive(false);
                }
            }
        }

        if (typePanel == "consumable")
        {
            for (int i = 0; i < consumableListItem.Count; i++)
            {
                consumableListItem[i].ID = i;
                shopConsumablePanelsItem[i].SetActive(true);

                shopConsumablePanels[i].rareItem.color = colorRare(consumableListItem[i].rarity.ToString());
                shopConsumablePanels[i].nameItem.text = consumableListItem[i].name;
                shopConsumablePanels[i].imgItem.sprite = consumableListItem[i].sprite;
                shopConsumablePanels[i].priceItem.text = consumableListItem[i].price.ToString();
            }
        }
    }

    public void ClickBlurBG()
    {
        itemDetail.SetActive(false);
        blurBG.SetActive(false);
    }
    public void CloseShop()
    {
        SceneManager.LoadScene("Choose World");
    }

    public void Purchased(int priceItem, int idItem, string typeItem)
    {
        if (priceItem > coin)
        {
            itemDetail.SetActive(false);
            blurBG.SetActive(false);
            notEnoughCoinBox.SetActive(true);
            GameObject.Find("Message").GetComponent<TMP_Text>().SetText("Not Enough Coin...");
            StartCoroutine(HideBox());
        }
        else
        {
            for (int i = 0; i < shopWeaponPanelsItem.Count; i++)
            {
                shopWeaponPanelsItem[i].SetActive(false);
                shopArmorPanelsItem[i].SetActive(false);
                shopConsumablePanelsItem[i].SetActive(false);
            }

            itemDetail.SetActive(false);
            blurBG.SetActive(false);
            notEnoughCoinBox.SetActive(true);

            //Update money
            coin -= priceItem;
            coinUI.text = coin.ToString();

            //Remove Item
            if (typeItem == "WEAPON")
            {
                weaponListItem[idItem].quantity -= 1;
                weaponListItem.RemoveAt(idItem);

                LoadPanels("weapon");
            }
            if (typeItem == "ARMOR")
            {
                armorListItem[idItem].quantity -= 1;
                armorListItem.RemoveAt(idItem);

                LoadPanels("armor");
            }
            if (typeItem == "CONSUMABLE")
            {
                consumableListItem[idItem].quantity += 1;
            }

            GameObject.Find("Message").GetComponent<TMP_Text>().SetText("Purchase Success");
            StartCoroutine(HideBox());
        }
    }

    private IEnumerator HideBox()
    {
        yield return new WaitForSeconds(displayTime);
        notEnoughCoinBox.SetActive(false);
    }
}