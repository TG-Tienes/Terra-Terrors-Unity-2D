using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Character", order = 1)]

public class PlayerStats : ScriptableObject
{
    public string charName = "name";

    // Base stats
    public int baseMaxHealth  = 0;
    public int baseHealth     = 0;
    public int baseAttack    = 0;
    public int baseDefense    = 0;

    // Runtime stats
    public int maxHealth      = 0;
    public int health        = 0;
    public int attack        = 0;
    public int defense      = 0;
}