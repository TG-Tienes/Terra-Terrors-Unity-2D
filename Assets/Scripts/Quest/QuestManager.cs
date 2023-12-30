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
            DontDestroyOnLoad(this);
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

    public void Reset()
    {
        enemiesKilled = 0;
        bossesKilled = 0;
    }

    public void RegisterEnemyKill()
    {
        enemiesKilled++;
        // Bạn có thể thêm các hành động khác tại đây, ví dụ như:
        // - Kiểm tra hoàn thành nhiệm vụ liên quan đến số lượng kẻ địch
        // - Cập nhật giao diện hiển thị số lượng kẻ địch bị tiêu diệt
    }

    public void RegisterBossKill()
    {
        bossesKilled++;
        // Bạn có thể thêm các hành động khác tại đây, ví dụ như:
        // - Kiểm tra hoàn thành nhiệm vụ liên quan đến boss
        // - Cập nhật giao diện hiển thị số lượng boss bị tiêu diệt
    }
}
