using UnityEngine;
using UnityEngine.UI;

public class ConsumableSlotClick : MonoBehaviour
{
    public Button button;

    public void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnSlotClick);
    }

    public void OnSlotClick()
    {
        PlayerControl.instance.UseConsumable();
    }
}