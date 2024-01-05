using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-10)]
public class ExpBar : MonoBehaviour
// Start is called before the first frame update
{
    public static ExpBar instance { get; private set; }

    float originalSize;
    public Image mask;
    private TMP_Text levelTitle;


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

        originalSize = mask.rectTransform.rect.width;
    }

    private void Start()
    {
        originalSize = mask.rectTransform.rect.width;
        levelTitle = GameObject.Find("LevelTitle").GetComponent<TMP_Text>();
    }

    public void SetValue(float value)
    {
        Debug.Log("exp: " + value.ToString());
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }

    public void SetLevel(int level)
    {
        levelTitle.SetText("LEVEL " + level.ToString());
    }
}

