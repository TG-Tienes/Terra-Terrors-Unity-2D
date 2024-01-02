using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotClick : MonoBehaviour
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
        if (slotIndex == 2)
        {
            if (PlayerControl.instance.consumableOnCooldown == false)
            {    
                PlayerControl.instance.UseConsumable();
            }
        }
        else
        {
            EquipmentManager.instance.currentWeapon = EquipmentManager.instance.currentEquipment[slotIndex + 3];
            WeaponSlotController.instance.UpdateWeaponSlots(slotIndex);
        }
    }
}