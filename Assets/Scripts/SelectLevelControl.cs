using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelControl : MonoBehaviour
{
    private string pathWorld1 = "Scenes/World/World 1/Levels/Level ";
    private string pathWorld2 = "Scenes/World/World 2/Levels/Level ";
    public Sprite mapLevel1World1;
    public Sprite mapLevel1World2;


    private string fullPathLevel;
    private int world;
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

        mapLevel = GameObject.Find("MapLevel").GetComponent<Image>();
        mission = GameObject.Find("DetailMap").GetComponent<TMP_Text>();


        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Choose Level World 1")
        {
            fullPathLevel = "Scenes/World/World 1/Levels/Level 1/Level 1 Scene";
            mapLevel.sprite = mapLevel1World1;
            world = 1;
        }
        if (currentScene.name == "Choose Level World 2")
        {
            fullPathLevel = "Scenes/World/World 2/Levels/Level 1/Level 1 Scene";
            mapLevel.sprite = mapLevel1World2;
            world = 2;
        }

        pressed = new Color(49 / 255f, 151 / 255f, 19 / 255f);
        normal = new Color(255 / 255f, 255 / 255f, 255 / 255f);

        btnPress = GameObject.Find("Level 1").GetComponent<Button>();
        changeColorButton(btnPress, pressed);
    }

    public void SetLevel(int level)
    {
        _buttonClicked.Play();

        if (world == 1)
            fullPathLevel = pathWorld1 + level.ToString() + "/Level " + level.ToString() + " scene";
        else
            fullPathLevel = pathWorld2 + level.ToString() + "/Level " + level.ToString() + " scene";

        mission.SetText(missionList[level - 1]);

        if (world == 1)
            for (int i = 0; i < 3; i++)
            {
                Button btn = GameObject.Find("Level " + (i + 1).ToString()).GetComponent<Button>();
                if (i != level - 1)
                    changeColorButton(btn, normal);
                else
                    changeColorButton(btn, pressed);
            }

        if (world == 2)
        {
            for (int i = 0; i < 1; i++)
            {
                Button btn = GameObject.Find("Level " + (i + 1).ToString()).GetComponent<Button>();
                if (i != level - 1)
                    changeColorButton(btn, normal);
                else
                    changeColorButton(btn, pressed);
            }
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
