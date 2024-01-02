using UnityEngine;
 
[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable", order = 1)]
 
public class Consumable : Item
{
    public int healthBoost;
    public int manaBoost;
 
    public override void Use()
    {
        base.Use();
        PlayerControl.instance.UseConsumable();
    }
}