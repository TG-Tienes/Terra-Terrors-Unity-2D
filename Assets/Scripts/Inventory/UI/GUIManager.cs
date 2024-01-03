using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class GUIManager : MonoBehaviour
{
    public Canvas playerInventory;

    #region Singleton
    public static GUIManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    // void Start()
    // {
    //     // playerInventory = GameObject.FindGameObjectWithTag("Player Inventory Canvas")?.GetComponent<Canvas>();
    //     Invoke("StartTimer", 2f);
    // }

    // void StartTimer()
    // {
    //     playerInventory.gameObject.SetActive(false);
    // }
}