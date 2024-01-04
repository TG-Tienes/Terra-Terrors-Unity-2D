using UnityEngine;
using UnityEngine.UI;

public class InventorySlotClick : MonoBehaviour
{
    public Button button;
    public int slotIndex;

    public float doubleClickThreshold = 0.3f;
    private float lastClickTime;

    public void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnSlotClick);
        button.onClick.AddListener(OnSlotDoubleClick);
    }

    public void OnSlotClick()
    {
        InventoryItemInfo.instance.slotIndex = slotIndex;
        InventoryItemInfo.instance.onSelectedItemChangedCallback?.Invoke();
    }

    public void OnSlotDoubleClick()
    {
        float currentTime = Time.time;
        if (currentTime - lastClickTime < doubleClickThreshold)
        {
            if (slotIndex < Inventory.instance.items.Count)
            {
                Item itemToBeUsed = Inventory.instance.items[slotIndex];
                if (itemToBeUsed != null)
                {
                    if (itemToBeUsed.type == ItemType.EQUIPMENT)
                    {
                        EquipmentManager.instance.Equip((Equipment) itemToBeUsed);
                        Inventory.instance.RemoveItem(itemToBeUsed);
                    }
                    else if (itemToBeUsed.type == ItemType.CONSUMABLE)
                    {
                        EquipmentManager.instance.EquipConsumable((Consumable) itemToBeUsed);
                        Inventory.instance.RemoveItem(itemToBeUsed);
                    }
                }
            }
        }

        lastClickTime = currentTime;
    }
}