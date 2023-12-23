using UnityEngine;
 
[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable", order = 1)]
 
public class Consumable : Item
{
    public int potency;
 
    public override void Use()
    {
        base.Use();
        Debug.Log("Using Item " + name + " with potency of " + potency);
    }
}