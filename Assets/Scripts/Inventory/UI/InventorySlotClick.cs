using UnityEngine;
using UnityEngine.UI;

public class InventorySlotClick : MonoBehaviour
{
    public Button button;
    public int slotIndex;

    public void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnSlotClick);
    }

    public void OnSlotClick()
    {
        InventoryItemInfo.instance.slotIndex = slotIndex;
        InventoryItemInfo.instance.onSelectedItemChangedCallback?.Invoke();
    }
}