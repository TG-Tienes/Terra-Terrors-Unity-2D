using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-50)]
public class ChooseWorldControl : MonoBehaviour, IDataPersistence
{
    NumberFormatter formatter;
    private TMP_Text coinUI;

    int coinCurrent;
    public void LoadData(GameData data)
    {
        this.coinCurrent = data.coin;
    }
    public void SaveData(ref GameData data)
    {
        data.coin = this.coinCurrent;
    }

    private void Start()
    {
        coinUI = GameObject.Find("CoinPlayer").GetComponent<TMP_Text>();
        formatter = new NumberFormatter();
        coinUI.text = formatter.FormatNumber(coinCurrent);
    }

    // Start is called before the first frame update
    public void BackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void GoToShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void GoToForest()
    {
        SceneManager.LoadScene("Choose Level World 1");
    }
}
