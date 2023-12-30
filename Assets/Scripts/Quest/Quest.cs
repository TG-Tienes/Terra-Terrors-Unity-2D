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


    [System.Serializable]
    public class Objective
    {
        public enum Type { killEnemy, killBoss, collect, talk }
        public int objectiveId;
        public int amount;
        [System.NonSerialized]
        public int currentAmount;
        public Type type;

        public bool CheckObjectiveCompleted(Type type, int id) {
            if (this.type == type && id == objectiveId)
                currentAmount++;
            return currentAmount >= amount;
        }

        public bool ForceAddObjective(int amount) {
            currentAmount += amount;
            return currentAmount >= amount && amount > 0;
        }

        public override string ToString() {
            switch (type) {
                case Type.killEnemy:
                    return "Kill " + /* MonsterList.MonsterNameFromID(objectiveId) + " " +*/ currentAmount + "/" + amount;
                case Type.killBoss:
                    return "Kill " + /* MonsterList.MonsterNameFromID(objectiveId) + " " +*/ currentAmount + "/" + amount;
                case Type.talk:
                    return "Talk to " /*+ NpcList.NpcNameFromID(objectiveId) */;
               case Type.collect:
                    return "Collect " + /* ItemList.ItemNameFromID(objectiveId) + " " +*/ currentAmount + "/" + amount;
            }
            return "";
        }
    }

}
