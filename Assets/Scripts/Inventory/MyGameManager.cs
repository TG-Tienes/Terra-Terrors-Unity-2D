using UnityEngine;

[DefaultExecutionOrder(-40)]
public class MyGameManager : MonoBehaviour
{
    #region Singleton
    public static MyGameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    void Start()
    {
        // Debug.Log("to sub");
        // EquipmentManager.instance.onEquipmentChangedCallback += EquipmentSlotController.instance.UpdateAllSlots;
        // Debug.Log("subbed");

        // EquipmentManager.instance.LoadEquipmentData();
    }
}