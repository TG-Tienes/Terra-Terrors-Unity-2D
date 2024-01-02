using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-50)]
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

    int coinCurrent;
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

    AudioSource _buttonClicked;

    NumberFormatter formatter;
    #region Singleton
    public static ShopManager instance;
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
        _buttonClicked = GameObject.Find("SceneAudioManager").gameObject.transform.GetChild(0).GetComponent<AudioSource>();

        Debug.Log(StatsManager.instance.playerStats.coin + "Coin current");

        coinUI = GameObject.Find("CoinPlayer").GetComponent<TMP_Text>();
        coinUI.text = formatter.FormatNumber(StatsManager.instance.playerStats.coin);

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
                weaponListItem[i].ID = i;
                if (weaponListItem[i].quantity != 0)
                {
                    shopWeaponPanelsItem[i].SetActive(true);

                    shopWeaponPanels[i].rareItem.color = colorRare(weaponListItem[i].rarity.ToString());
                    shopWeaponPanels[i].nameItem.text = weaponListItem[i].name;
                    shopWeaponPanels[i].imgItem.sprite = weaponListItem[i].sprite;
                    shopWeaponPanels[i].priceItem.text = formatter.FormatNumber(weaponListItem[i].price);
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
                armorListItem[i].ID = i;
                if (armorListItem[i].quantity != 0)
                {
                    shopArmorPanelsItem[i].SetActive(true);

                    shopArmorPanels[i].rareItem.color = colorRare(armorListItem[i].rarity.ToString());
                    shopArmorPanels[i].nameItem.text = armorListItem[i].name;
                    shopArmorPanels[i].imgItem.sprite = armorListItem[i].sprite;
                    shopArmorPanels[i].priceItem.text = formatter.FormatNumber(armorListItem[i].price);
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
                shopConsumablePanels[i].priceItem.text = formatter.FormatNumber(consumableListItem[i].price);
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
        _buttonClicked.Play();
        Invoke("OpenChooseWorldScene", _buttonClicked.clip.length);
    }

    public void OpenChooseWorldScene()
    {
        SceneManager.LoadScene("Choose World");
    }

    public void Purchased(Item currentItem, int priceItem, int idItem, string typeItem)
    {
        if (priceItem > StatsManager.instance.playerStats.coin)
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
            }

            itemDetail.SetActive(false);
            blurBG.SetActive(false);
            notEnoughCoinBox.SetActive(true);

            Item itemCopy = Instantiate(currentItem);
            Inventory.instance.AddItem(itemCopy);

            //Update money
            StatsManager.instance.playerStats.coin -= priceItem;
            coinUI.text = formatter.FormatNumber(StatsManager.instance.playerStats.coin);
            //Remove Item
            if (typeItem == "WEAPON")
            {
                weaponListItem[idItem].quantity -= 1;
                LoadPanels("weapon");
            }
            if (typeItem == "ARMOR")
            {
                armorListItem[idItem].quantity -= 1;

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