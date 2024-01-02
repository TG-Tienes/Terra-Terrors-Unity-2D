using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Character", order = 1)]

public class PlayerStats : ScriptableObject
{
    public string charName = "name";

    public int coin = 0;
    public int level = 1;
    public int health = 0;
    public int attack = 0;
    public int defense = 0;
    public int mana = 0;
    public int exp = 0;
    public float fireRate = 0;

    // Load data from JSON
    public static PlayerStats LoadFromJson(string jsonString, PlayerStats existingInstance = null)
    {
        PlayerStats data;

        if (existingInstance != null)
        {
            data = existingInstance;
        }
        else
        {
            data = CreateInstance<PlayerStats>();
        }

        JsonUtility.FromJsonOverwrite(jsonString, data);
        return data;
    }
}