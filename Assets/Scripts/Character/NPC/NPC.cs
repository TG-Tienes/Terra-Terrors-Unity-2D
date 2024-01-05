using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class NPC : MonoBehaviour {
   public GameObject dialoguePanel;
   public TMP_Text dialogueText;
   public string[] dialogue;
   private int index;

   public float wordSpeed;
   public bool playerIsClose = false;
   public GameObject contButton;
   bool isTalk;

  void Update()
  {
   if (!isTalk) {
     if (Input.GetKeyDown(KeyCode.E) && playerIsClose) 
    {
      if(dialoguePanel.activeInHierarchy)
      {
        zeroText();
      }
      else {
        dialoguePanel.SetActive(true);
        StartCoroutine(Typing());
      }
    }

    if(dialogueText.text == dialogue[index])
    {
      contButton.SetActive(true);
    }
   }
  }

  public void zeroText()
  {
    dialogueText.text = "";
    index = 0;
    dialoguePanel.SetActive(false);
  }

  IEnumerator Typing()
  {
    foreach(char letter in dialogue[index].ToCharArray())
    {
      dialogueText.text += letter;
      yield return new WaitForSeconds(wordSpeed);
    }
  }

  public void NextLine()
  {
    contButton.SetActive(false);
    
    if(index < dialogue.Length - 1)
    {
      index++;
      dialogueText.text = "";
      StartCoroutine(Typing());
    }
    else {
      zeroText();
      QuestManager.instance.RegisterTalkNPC();
      QuestLog.CheckQuestObjective(Quest.Objective.Type.talk, QuestManager.instance.talkNPC);
      isTalk = true;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.gameObject.tag.Equals("Main Character")) {
      playerIsClose = true;
    }
  }

  private void OnTriggerExit2D(Collider2D collision) {
    if(collision.gameObject.tag.Equals("Main Character")) {
      playerIsClose = false;
      zeroText();
    }
  }
}