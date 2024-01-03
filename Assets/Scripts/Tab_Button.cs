using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

// Add an alias for the UnityEngine.UI.Image class
using UnityImage = UnityEngine.UI.Image;
using UnityEngine.Events;

[RequireComponent(typeof(UnityImage))]
public class Tab_Button : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;
    public UnityImage background;

    // public UnityEvent onTabSelect;
    // public UnityEvent onTabDeselect;
    AudioSource _buttonClicked;

    private void Start()
    {
        _buttonClicked = GameObject.Find("SceneAudioManager").gameObject.transform.GetChild(3).GetComponent<AudioSource>();


        background = GetComponent<UnityImage>();
        tabGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _buttonClicked.Play();
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }
}