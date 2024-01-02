using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestLog 
{
    private static List<Quest> questList;
    private static List<Quest> completedQuest;

    public delegate void OnQuestChange(List<Quest> activeQuests, List<Quest> completedQuest);
    public static event OnQuestChange onQuestChange;

    public static void Initialize() {

        questList = new List<Quest>();
        completedQuest = new List<Quest>();
        // QuestManager.instance.Reset(); 
    }

    public static void AddQuest(Quest quest) {
        questList.Add(quest);
        // HandleOwnedItems(quest);
        onQuestChange.Invoke(questList, completedQuest);
    }

    public static void CheckQuestObjective(Quest.Objective.Type type, int quantity) {
        List<Quest> questsToComplete = new List<Quest>();
        foreach (Quest quest in questList)
        {
            if (quest.objective.CheckObjectiveCompleted(type, quantity))
            {
                Debug.Log(StatsManager.instance.playerStats.coin);
                Debug.Log(quest.expReward);
                Debug.Log(quest.goldReward);
                // StatsManager.instance.playerStats.coin += quest.goldReward;
                // StatsManager.instance.playerStats.exp += quest.expReward;
                PlayerControl.instance.handleCoin(quest.goldReward);
                PlayerControl.instance.handleExp(quest.expReward);
                Debug.Log(StatsManager.instance.playerStats.coin);
                questsToComplete.Add(quest);
            }
        }

        foreach (Quest quest in questsToComplete)
        {
            CompleteQuest(quest);
        }
        onQuestChange.Invoke(questList, completedQuest);

    }

    public static void CompleteQuest(Quest quest) {
        questList.Remove(quest);
        completedQuest.Add(quest);
        // Inventory.giveGold(quest.goldReward);
        // Character.giveExp(quest.expReward);
        onQuestChange.Invoke(questList, completedQuest);

    }

    // private static void HandleOwnedItems(Quest quest) {
    //     if (quest.objective.type != Quest.Objective.Type.collect)
    //         return;
    //     int amount = 0;//Inventory.GetCountOfIndex(quest.objective.objectiveId); 
    //     if (quest.objective.ForceAddObjective(amount))
    //         CompleteQuest(quest);
    // }

    public static Quest getQuestNo(int index) {
        if (index < questList.Count)
            return questList[index];
        else
            return completedQuest[index - questList.Count];
    }


}