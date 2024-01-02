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
            Debug.Log("inven d-click");
            Item itemToBeUsed = Inventory.instance.items[slotIndex];
            if (itemToBeUsed != null)
            {
                if (itemToBeUsed.type == ItemType.EQUIPMENT)
                {
                    Debug.Log("inven EQUIPMENT click");
                    EquipmentManager.instance.Equip((Equipment) itemToBeUsed);
                    Inventory.instance.RemoveItem(itemToBeUsed);
                }
                else if (itemToBeUsed.type == ItemType.CONSUMABLE)
                {
                    Debug.Log("inven CONSUMABLE click");
                    EquipmentManager.instance.EquipConsumable((Consumable) itemToBeUsed);
                    Inventory.instance.RemoveItem(itemToBeUsed);
                }
            }
        }

        lastClickTime = currentTime;
    }
}