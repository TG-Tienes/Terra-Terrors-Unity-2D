using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int level;
    public int blood;
    public int exp;
    public int mana;
    public int coin;

    public PlayerStats playerStats;

    public GameData()
    {
        this.level = 1;
        this.blood = 100;
        this.exp = 0;
        this.mana = 50;
        this.coin = 0;
    }
    public GameData(int level, int blood, int exp, int mana, int coin)
    {
        this.level = level;
        this.blood = blood;
        this.exp = exp;
        this.mana = mana;
        this.coin = coin;
    }

    public override string ToString()
    {
        return $"Level: {level}, HP: {blood}, EXP: {exp}, MANA: {mana}, COIN: {coin}";
    }
}