using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelControl : MonoBehaviour
{
    private string path = "Scenes/World/World 1/Levels/Level ";
    private string fullPathLevel;
    private Image mapLevel;
    private Button btnPress;
    private Color normal;
    private Color pressed;
    private TMP_Text mission;
    private string[] missionList = {"Mission: to navigate the secrets of the forest, confront formidable challenges, and reclaim the invaluable key, all while facing the lurking danger posed by the enigmatic creature.",
     "Mission: to explore the secrets of the forest, confront formidable challenges, and reclaim the invaluable key, all while facing the lurking danger posed by the enigmatic creature.",
     "Mission: to discover the secrets of the forest, confront formidable challenges, and reclaim the invaluable key, all while facing the lurking danger posed by the enigmatic creature."   };

    AudioSource _buttonClicked;
    AudioSource _navButtonClicked;
    
    void Start()
    {
        _buttonClicked = GameObject.Find("SceneAudioManager").gameObject.transform.GetChild(1).GetComponent<AudioSource>();
        _navButtonClicked = GameObject.Find("SceneAudioManager").gameObject.transform.GetChild(0).GetComponent<AudioSource>();

        fullPathLevel = "Scenes/World/World 1/Levels/Level 1/Level 1 Scene";
        mapLevel = GameObject.Find("MapLevel").GetComponent<Image>();
        mission = GameObject.Find("DetailMap").GetComponent<TMP_Text>();

        pressed = new Color(49 / 255f, 151 / 255f, 19 / 255f);
        normal = new Color(255 / 255f, 255 / 255f, 255 / 255f);

        btnPress = GameObject.Find("Level 1").GetComponent<Button>();
        changeColorButton(btnPress, pressed);

    }

    public void SetLevel(int level)
    {
        _buttonClicked.Play();

        fullPathLevel = path + level.ToString() + "/Level " + level.ToString() + " scene";
        mission.SetText(missionList[level - 1]);

        for (int i = 0; i < 3; i++)
        {
            Button btn = GameObject.Find("Level " + (i + 1).ToString()).GetComponent<Button>();
            if (i != level - 1)
                changeColorButton(btn, normal);
            else
                changeColorButton(btn, pressed);
        }
    }

    public void SetImageLevel(Sprite mapSpriteLevel)
    {
        mapLevel.sprite = mapSpriteLevel;
    }

    public void PlayGame()
    {
        _navButtonClicked.Play();
        SceneManager.LoadScene(fullPathLevel);
    }

    public void BackGame()
    {
        Invoke("BackToChooseWorld", _navButtonClicked.clip.length);
        _navButtonClicked.Play();
        //SceneManager.LoadScene(2);
    }

    public void BackToChooseWorld()
    {
        SceneManager.LoadScene("Choose World");
    }

    private void changeColorButton(Button btn, Color newColor)
    {
        btn.GetComponent<Image>().color = newColor;
    }
}
