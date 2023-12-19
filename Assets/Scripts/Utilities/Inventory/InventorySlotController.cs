// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using System;
// using UnityEditor.Build.Content;

// [DefaultExecutionOrder(-50)]
// public class InventorySlotController : MonoBehaviour
// {
//     public List<TextMeshProUGUI> slots;
 
//     private void Awake()
//     {

//     }

//     private void Start()
//     {
//         UpdateSlots();
//     }
     
//     void UpdateSlots()
//     {
//         Type fieldsType = typeof(PlayerStats);
    
//         foreach (TextMeshProUGUI field in fields)
//         {
//             string value = fieldsType.GetField(field.name).GetValue(StatsManager.instance.playerStats).ToString();
//             field.text = value;       
//         }
//     }
// }