using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-50)]
public class ShopManagerInGame : MonoBehaviour
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

    int coinCurrent;
    private TMP_Text coinUI;
    public List<Equipment> armorListItem;
    public List<Equipment> weaponListItem;
    public List<Consumable> consumableListItem;


    public List<GameObject> shopPanelsItem;
    public List<ShopTemplate> shopPanels;

    public GameObject blurBG;
    public GameObject boxConfirm;

    public float displayTime = 2f;
    public GameObject notEnoughCoinBox;
    NumberFormatter formatter;
    #region Singleton
    public static ShopManagerInGame instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            formatter = new NumberFormatter();
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
        coinUI.text = formatter.FormatNumber(StatsManager.instance.playerStats.coin);

        PlayerPrefs.SetInt("idWeapon", -1);
        PlayerPrefs.SetInt("idArmor", -1);
        PlayerPrefs.SetInt("idConsumable", -1);
        PlayerPrefs.Save();
        LoadRandomItems();
    }

    public void LoadRandomItems()
    {
        //Random weapon
        int idWeapon = PlayerPrefs.GetInt("idWeapon", -1);
        if (idWeapon == -1)
        {
            idWeapon = UnityEngine.Random.Range(0, weaponListItem.Count);

            while (weaponListItem[idWeapon].quantity == 0)
                idWeapon = UnityEngine.Random.Range(0, weaponListItem.Count);

            PlayerPrefs.SetInt("idWeapon", idWeapon);
        }

        if (weaponListItem[idWeapon].quantity == 0)
            shopPanelsItem[0].SetActive(false);
        else
        {
            shopPanelsItem[0].SetActive(true);

            shopPanels[0].rareItem.color = colorRare(weaponListItem[idWeapon].rarity.ToString());
            shopPanels[0].nameItem.text = weaponListItem[idWeapon].name;
            shopPanels[0].imgItem.sprite = weaponListItem[idWeapon].sprite;
            shopPanels[0].attackData.text = "Attack: +" + weaponListItem[idWeapon].attackModifier.ToString();
            shopPanels[0].defendData.text = "Defense: +" + weaponListItem[idWeapon].defenseModifier.ToString();
            shopPanels[0].priceItem.text = formatter.FormatNumber(weaponListItem[idWeapon].price);
        }


        //Random Armor
        int idArmor = PlayerPrefs.GetInt("idArmor", -1);
        if (idArmor == -1)
        {
            idArmor = UnityEngine.Random.Range(2000, 2000 + armorListItem.Count);

            while (armorListItem[idArmor - 2000].quantity == 0)
                idArmor = UnityEngine.Random.Range(2000, 2000 + armorListItem.Count);

            PlayerPrefs.SetInt("idArmor", idArmor);
        }

        if (armorListItem[idArmor - 2000].quantity == 0)
            shopPanelsItem[1].SetActive(false);
        else
        {
            shopPanelsItem[1].SetActive(true);

            shopPanels[1].rareItem.color = colorRare(armorListItem[idArmor - 2000].rarity.ToString());
            shopPanels[1].nameItem.text = armorListItem[idArmor - 2000].name;
            shopPanels[1].imgItem.sprite = armorListItem[idArmor - 2000].sprite;
            shopPanels[1].attackData.text = "Attack: +" + armorListItem[idArmor - 2000].attackModifier.ToString();
            shopPanels[1].defendData.text = "Defense: +" + armorListItem[idArmor - 2000].defenseModifier.ToString();
            shopPanels[1].priceItem.text = formatter.FormatNumber(armorListItem[idArmor - 2000].price);
        }

        //Random Consumable
        int idConsumable = PlayerPrefs.GetInt("idConsumable", -1);
        if (idConsumable == -1)
        {
            idConsumable = UnityEngine.Random.Range(1000, 1000 + consumableListItem.Count);
            PlayerPrefs.SetInt("idConsumable", idConsumable);
        }

        shopPanelsItem[2].SetActive(true);
        shopPanels[2].rareItem.color = colorRare(consumableListItem[idConsumable - 1000].rarity.ToString());
        shopPanels[2].nameItem.text = consumableListItem[idConsumable - 1000].name;
        shopPanels[2].imgItem.sprite = consumableListItem[idConsumable - 1000].sprite;
        shopPanels[2].attackData.text = "Modificafication: +" + consumableListItem[idConsumable - 1000].healthBoost.ToString();
        shopPanels[2].priceItem.text = formatter.FormatNumber(consumableListItem[idConsumable - 1000].price);
        PlayerPrefs.Save();

    }

    public void ResetUIState()
    {
        notEnoughCoinBox.SetActive(false);
        // Other UI elements you want to reset
    }

    public void Purchased(Item currentItem, int priceItem, int idItem, string typeItem)
    {
        if (priceItem > StatsManager.instance.playerStats.coin)
        {
            blurBG.SetActive(false);
            notEnoughCoinBox.SetActive(true);
            GameObject.Find("Message").GetComponent<TMP_Text>().SetText("Not Enough Coin...");
            StartCoroutine(HideBox());
        }
        else
        {
            boxConfirm.SetActive(false);
            blurBG.SetActive(false);
            notEnoughCoinBox.SetActive(true);

            Item itemCopy = Instantiate(currentItem);
            Debug.Log(itemCopy.ID);
            Inventory.instance.AddItem(itemCopy);

            //Update money
            StatsManager.instance.playerStats.coin -= priceItem;
            coinUI.text = formatter.FormatNumber(StatsManager.instance.playerStats.coin);

            //Remove Item
            if (typeItem == "WEAPON")
            {
                currentItem.quantity -= 1;
                weaponListItem[idItem].quantity -= 1;
                shopPanelsItem[0].SetActive(false);
            }
            if (typeItem == "ARMOR")
            {
                currentItem.quantity -= 1;
                armorListItem[idItem - 2000].quantity -= 1;
                shopPanelsItem[1].SetActive(false);
            }
            if (typeItem == "CONSUMABLE")
            {
                consumableListItem[idItem - 1000].quantity += 1;
                shopPanelsItem[2].SetActive(false);
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