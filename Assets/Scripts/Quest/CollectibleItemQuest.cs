using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItemQuest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();
        if (player != null)
        {
            Destroy(gameObject);
            QuestManager.instance.RegisterCollectItem();
            QuestLog.CheckQuestObjective(Quest.Objective.Type.collect, QuestManager.instance.collectItem);

        }
    }
}


