using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuController : MonoBehaviour
{
    public Toggle fullscreentoggle; 

    public void toggleFullscreen()
    {
       Screen.fullScreen = fullscreentoggle.isOn;
    }
}
