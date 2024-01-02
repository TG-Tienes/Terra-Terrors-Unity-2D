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

    AudioSource _buttonClickedAudio;
    AudioSource _openShopAudio;
    AudioSource _openInventoryAudio;
    AudioSource _chooseWorldAudio;

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
        GameObject sceneAudioManager = GameObject.Find("SceneAudioManager").gameObject;

        _buttonClickedAudio = sceneAudioManager.transform.GetChild(0).GetComponent<AudioSource>();
        _openShopAudio = sceneAudioManager.transform.GetChild(5).GetComponent<AudioSource>();
        _openInventoryAudio = sceneAudioManager.transform.GetChild(3).GetComponent<AudioSource>();
        _chooseWorldAudio = sceneAudioManager.transform.GetChild(6).GetComponent<AudioSource>();

        coinUI = GameObject.Find("CoinPlayer").GetComponent<TMP_Text>();
        formatter = new NumberFormatter();
        coinUI.text = formatter.FormatNumber(coinCurrent);
    }

    // Start is called before the first frame update
    public void BackToMenu()
    {
        _buttonClickedAudio.Play();
        SceneManager.LoadScene("Main Menu");
    }

    public void GoToShop()
    {
        _openShopAudio.Play();
        SceneManager.LoadScene("Shop");
    }

    public void GoToForest()
    {
        _chooseWorldAudio.Play();
        SceneManager.LoadScene("Choose Level World 1");
    }
}
