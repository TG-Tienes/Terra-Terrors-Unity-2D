using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    #region Singleton
    public static QuestManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Reset();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public int enemiesKilled = 0;
    public int bossesKilled = 0;
    public int collectItem = 0;
    public int talkNPC = 0;


    public void Reset()
    {
        enemiesKilled = 0;
        bossesKilled = 0;
        collectItem = 0;
        talkNPC = 0;
    }

    public void RegisterEnemyKill()
    {
        enemiesKilled++;
    }

    public void RegisterBossKill()
    {
        bossesKilled++;
    }

    public void RegisterCollectItem()
    {
        collectItem++;
    }

    public void RegisterTalkNPC()
    {
        talkNPC++;
    }
}
