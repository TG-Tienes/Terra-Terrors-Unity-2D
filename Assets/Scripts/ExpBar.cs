using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
// Start is called before the first frame update
{
    public static ExpBar instance { get; private set; }

    float originalSize;
    public Image mask;
    private TMP_Text levelTitle;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        originalSize = mask.rectTransform.rect.width;
        levelTitle = GameObject.Find("LevelTitle").GetComponent<TMP_Text>();
    }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }

    public void SetLevel(int level)
    {
        levelTitle.SetText("LEVEL " + level.ToString());
    }
}
