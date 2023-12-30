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
    public int manaUsage;
    public float fireRate;
    public float criticalChance;

    public override void Use()
    {
        base.Use();
        EquipmentManager.instance.Equip(this);
        Inventory.instance.RemoveItem(this);
    }

    // Load data from JSON
    public static Equipment LoadEquipmentFromJson(string jsonString)
    {
        Equipment data = CreateInstance<Equipment>();
        JsonUtility.FromJsonOverwrite(jsonString, data);
        return data;
    }
}