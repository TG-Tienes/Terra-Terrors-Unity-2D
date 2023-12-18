using UnityEngine;

public enum EquipType
{
    HEAD,
    CHEST,
    LEGS,
    WEAPON
}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment", order = 1)]

public class Equipment : Item
{
    public EquipType equipType;
 
    public int attackModifier;
    public int defenseModifier;

    public override void Use()
    {
        base.Use();
        EquipmentManager.instance.Equip(this);
        Inventory.instance.RemoveItem(this);
    }
}