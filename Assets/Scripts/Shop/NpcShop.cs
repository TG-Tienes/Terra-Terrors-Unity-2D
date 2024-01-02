using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class NpcShop : MonoBehaviour
{
    public GameObject questionBox;
    public GameObject blurBG;
    public GameObject messageBox;
    public GameObject ShopInGame;

    // Start is called before the first frame update
    void Start()
    {
        questionBox.SetActive(false);
    }
    public void ShowQuestionBox()
    {
        questionBox.SetActive(true);
    }

    public void HideQuestionBox()
    {
        questionBox.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Main Character"))
        {
            ShowQuestionBox();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Main Character"))
        {
            HideQuestionBox();
        }
    }

    public void onClickQuestionBox()
    {
        GameObject.Find("QuestionButton").SetActive(false);
        blurBG.SetActive(true);
        messageBox.SetActive(true);
        MessageController.instance.ResetSentence();
        MessageController.instance.NextSentence();
        PauseMenuManager.pauseGame();
    }

    public void ClickBlurBG()
    {
        blurBG.SetActive(false);
        messageBox.SetActive(false);
        ShopInGame.SetActive(false);
        PauseMenuManager.unpauseGame();
    }

    public void GoToShop()
    {
        blurBG.SetActive(true);
        messageBox.SetActive(false);
        ShopInGame.SetActive(true);
        ShopManagerInGame.instance.ResetUIState();
    }


    public void CloseShop()
    {
        blurBG.SetActive(false);
        ShopInGame.SetActive(false);
        PauseMenuManager.unpauseGame();
    }
}
