using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotClick : MonoBehaviour
{
    public Button button;
    public int slotIndex;

    public float doubleClickThreshold = 0.3f;
    private float lastClickTime;

    public void Start()
    {
        button = GetComponent<Button>();
        // button.onClick.AddListener(OnSlotClick);
        button.onClick.AddListener(OnSlotDoubleClick);
    }

    // public void OnSlotClick()
    // {
    //     InventoryItemInfo.instance.slotIndex = slotIndex;
    //     InventoryItemInfo.instance.onSelectedItemChangedCallback?.Invoke();
    // }

    public void OnSlotDoubleClick()
    {
        float currentTime = Time.time;

        if (currentTime - lastClickTime < doubleClickThreshold)
        {
            Equipment equipmentToBeRemoved = EquipmentManager.instance.currentEquipment[slotIndex];
            EquipmentManager.instance.Unequip(equipmentToBeRemoved);
        }

        lastClickTime = currentTime;
    }
}