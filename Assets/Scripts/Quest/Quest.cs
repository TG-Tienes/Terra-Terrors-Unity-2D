using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest 
{
    public string questName;
    public string questDescription;
    public int goldReward;
    public int expReward;
    public Objective objective;
    public short questCategory;
    public string imagePath;

    [System.Serializable]
    public class Objective
    {
        public enum Type { killEnemy, killBoss, collect, talk }
        public int objectiveId;
        public int amount ;
        [System.NonSerialized]
        public int currentAmount;
        public Type type;

        public bool CheckObjectiveCompleted(Type type, int quantity) {
            Debug.Log("type: " + type);
            if (this.type == type) {
                Debug.Log("type check: " + type);
                currentAmount = quantity;

            }
            Debug.Log("type check: " + type + ": " + currentAmount + "amount: " + amount);
            return currentAmount >= amount;
        }

        // public bool ForceAddObjective(int amount) {
        //     currentAmount += amount;
        //     return currentAmount >= amount && amount > 0;
        // }

        public override string ToString() {
            switch (type) {
                case Type.killEnemy:
                    return "Kill " + /* MonsterList.MonsterNameFromID(objectiveId) + " " +*/ currentAmount + "/" + amount;
                case Type.killBoss:
                    return "Kill " + /* MonsterList.MonsterNameFromID(objectiveId) + " " +*/ currentAmount + "/" + amount;
                case Type.talk:
                    return "Talk to " + " Hera";
               case Type.collect:
                    return "Collect " + /* ItemList.ItemNameFromID(objectiveId) + " " +*/ currentAmount + "/" + amount;
            }
            return "";
        }
    }

}
