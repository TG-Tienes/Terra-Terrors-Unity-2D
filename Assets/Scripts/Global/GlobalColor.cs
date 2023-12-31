using UnityEngine;

public class GlobalColor : MonoBehaviour
{
    // Inventory slot color
    public static Color32 color_SlotDefault = new Color32(137, 82, 59, 255);
    // Item rarity color
    public static Color32 color_Common    = new Color32(222, 222, 222, 255);
    public static Color32 color_Uncommon  = new Color32(45, 142, 59, 255);
    public static Color32 color_Rare      = new Color32(0, 152, 255, 255);
    public static Color32 color_Epic      = new Color32(198, 0, 230, 255);
    public static Color32 color_Legendary = new Color32(255, 102, 0, 255);
    public static Color32 color_Mythical  = new Color32(207, 50, 50, 255);

    public static Color32 color_weaponEnabled = new Color32(255,255,255,100);
    public static Color32 color_weaponDisabled = new Color32(0,0,0,170);
}