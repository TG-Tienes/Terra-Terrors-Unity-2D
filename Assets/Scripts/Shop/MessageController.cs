using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    public TMP_Text message;
    public string[] sentence;
    int index = 0;
    public float messageSpeed;

    #region Singleton
    public static MessageController instance;
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
    }

    #endregion Singleton

    // Update is called once per frame
    public void NextSentence()
    {
        if (index <= sentence.Length - 1)
        {
            message.text = "";
            StartCoroutine(WriteSentence());
        }
        index++;
    }

    public void ResetSentence()
    {
        index = 0;
    }
    IEnumerator WriteSentence()
    {
        foreach (char character in sentence[index].ToCharArray())
        {
            message.text += character;
            yield return new WaitForSecondsRealtime(messageSpeed);
        }
    }
}
