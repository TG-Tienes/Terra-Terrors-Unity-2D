using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;  
using System.Linq;

[DefaultExecutionOrder(50)]
public class InventorySlotController : MonoBehaviour
{
    public List<Image> spriteFields = new List<Image>();
    public List<TextMeshProUGUI> quantityFields = new List<TextMeshProUGUI>();
    public List<Image> rarityBackgroundFields = new List<Image>();

    #region Singleton
    public static InventorySlotController instance;
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
        Inventory.instance.onItemChangedCallback += UpdateAllSlots;
        UpdateAllSlots();
    }

    public void OnDestroy()
    {
        if (Inventory.instance != null)
        {
            Inventory.instance.onItemChangedCallback -= UpdateAllSlots;
        }
    }

    public void GetFields()
    {
        spriteFields.Clear();
        GameObject[] objects_1 = GameObject.FindGameObjectsWithTag("InventorySlotSpriteField");
        List<GameObject> sorted_1 = objects_1.OrderBy(obj => obj.name).ToList();
        foreach (GameObject obj in sorted_1)
        {
            Image result = obj.GetComponent<Image>();
            if (result != null)
            {
                spriteFields.Add(result);
            }
        }

        quantityFields.Clear();
        GameObject[] objects_2 = GameObject.FindGameObjectsWithTag("InventorySlotQuantityField");
        List<GameObject> sorted_2 = objects_2.OrderBy(obj => obj.name).ToList();
        foreach (GameObject obj in sorted_2)
        {
            TextMeshProUGUI result = obj.GetComponent<TextMeshProUGUI>();
            if (result != null)
            {   
                quantityFields.Add(result);
            }
        }

        rarityBackgroundFields.Clear();
        GameObject[] objects_3 = GameObject.FindGameObjectsWithTag("InventorySlotRarityBackground");
        List<GameObject> sorted_3 = objects_3.OrderBy(obj => obj.name).ToList();
        foreach (GameObject obj in sorted_3)
        {
            Image result = obj.GetComponent<Image>();
            if (result != null)
            {
                rarityBackgroundFields.Add(result);
            }
        }
    }

    public void SetActiveAndUpdateSlot(int index, Sprite sprite, int quantity, ItemRarity rarity)
    {
        spriteFields[index].color = new Color32(255,255,255,255);
        spriteFields[index].sprite = sprite;

        quantityFields[index].transform.parent.gameObject.GetComponent<Image>().enabled = true;
        quantityFields[index].text = quantity.ToString();

        switch (rarity)
        {
            case ItemRarity.COMMON:
            {
                rarityBackgroundFields[index].color = GlobalColor.color_Common;
                break;
            }
            case ItemRarity.UNCOMMON:
            {
                rarityBackgroundFields[index].color = GlobalColor.color_Uncommon;
                break;
            }
            case ItemRarity.RARE:
            {
                rarityBackgroundFields[index].color = GlobalColor.color_Rare;
                break;
            }
            case ItemRarity.EPIC:
            {
                rarityBackgroundFields[index].color = GlobalColor.color_Epic;
                break;
            }
            case ItemRarity.LEGENDARY:
            {
                rarityBackgroundFields[index].color = GlobalColor.color_Legendary;
                break;
            }
            case ItemRarity.MYTHICAL:
            {
                rarityBackgroundFields[index].color = GlobalColor.color_Mythical;
                break;
            }
            default:
                break;
        };
    }

    public void UpdateAllSlots()
    {
        ClearAllSlots();

        foreach (Item item in Inventory.instance.items)
        {
            // if (item.quantity > Inventory.instance.stackSize)
            // {
            //     for (int iteration = item.quantity / Inventory.instance.stackSize; iteration >= 0; iteration--)
            //     {
            //         if (iteration > 0)
            //         {
            //             SetActiveAndUpdateSlot(index, item.sprite, Inventory.instance.stackSize, item.rarity);
            //         }
            //         else
            //         {
            //             if (item.quantity % Inventory.instance.stackSize != 0)
            //             {   
            //                 SetActiveAndUpdateSlot(index, item.sprite, item.quantity % Inventory.instance.stackSize, item.rarity);
            //             }
            //         }
            //         index++;
            //     }
            // }
            SetActiveAndUpdateSlot(Inventory.instance.items.IndexOf(item), item.sprite, item.quantity, item.rarity);
        }
    }

    public void ClearAllSlots()
    {
        foreach (Image spriteField in spriteFields)
        {
            spriteField.color = new Color32(255,255,255,0);
        }
        foreach (TextMeshProUGUI quantityField in quantityFields)
        {
            quantityField.transform.parent.gameObject.GetComponent<Image>().enabled = false;
            quantityField.text = "";
        }
        foreach (Image rarityBackgroundField in rarityBackgroundFields)
        {
            if (rarityBackgroundField != null)
                rarityBackgroundField.color = GlobalColor.color_SlotDefault;
        }
    }
}