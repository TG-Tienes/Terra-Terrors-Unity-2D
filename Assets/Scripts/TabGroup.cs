using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this line

public class TabGroup : MonoBehaviour
{
    Color tabHover = new Color(200 / 255f, 200 / 255f, 200 / 255f);
    Color tabIdle = new Color(255 / 255f, 255 / 255f, 255 / 255f);
    Color tabActive = new Color(161 / 255f, 161 / 255f, 161 / 255f);

    public List<Tab_Button> tab_Buttons;
    public Tab_Button selectedTab;

    public List<GameObject> objectsToSwap;

    public void Subscribe(Tab_Button button)
    {
        if (tab_Buttons == null)
            tab_Buttons = new List<Tab_Button>();

        tab_Buttons.Add(button);
    }

    public void OnTabEnter(Tab_Button button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
            button.background.color = tabHover;
    }

    public void OnTabExit(Tab_Button button)
    {
        ResetTabs();
    }

    public void OnTabSelected(Tab_Button button)
    {
        // if (selectedTab != null)
        //     selectedTab.Deselect();

        // selectedTab = button;
        // selectedTab.Select();

        selectedTab = button;

        ResetTabs();
        button.background.color = tabActive;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);

                if (objectsToSwap[i].name == "ArmorScroll")
                    ShopManager.instance.LoadPanels("armor");
                if (objectsToSwap[i].name == "WeaponScroll")
                    ShopManager.instance.LoadPanels("weapon");
                if (objectsToSwap[i].name == "ConsumableScroll")
                    ShopManager.instance.LoadPanels("consumable");
            }
            else
                objectsToSwap[i].SetActive(false);
        }
    }

    public void ResetTabs()
    {
        foreach (Tab_Button button in tab_Buttons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
            button.background.color = tabIdle;
        }
    }
}