using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotClick : MonoBehaviour
{
    public Button button;
    public int slotIndex;

    public float doubleClickThreshold = 0.3f;
    private float lastClickTime;
    AudioSource _unequipAudio;

    public void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnSlotDoubleClick);

        _unequipAudio = GameObject.Find("SceneAudioManager").gameObject.transform.GetChild(2).GetComponent<AudioSource>();
    }

    public void OnSlotDoubleClick()
    {
        float currentTime = Time.time;

        if (currentTime - lastClickTime < doubleClickThreshold)
        {
            _unequipAudio.Play();
            Equipment equipmentToBeRemoved = EquipmentManager.instance.currentEquipment[slotIndex];
            EquipmentManager.instance.Unequip(slotIndex, equipmentToBeRemoved);
        }

        lastClickTime = currentTime;
    }
}