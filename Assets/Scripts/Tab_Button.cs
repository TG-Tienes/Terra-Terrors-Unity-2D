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

    private void Start()
    {
        background = GetComponent<UnityImage>();
        tabGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
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

    // public void Select()
    // {
    //     if (onTabSelect != null)
    //         onTabSelect.Invoke();
    // }

    // public void Deselect()
    // {
    //     if (onTabDeselect != null)
    //         onTabDeselect.Invoke();
    // }
}