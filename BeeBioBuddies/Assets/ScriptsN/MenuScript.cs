using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public void SingleButton()
    {
        SceneManager.LoadScene(1); // 1. sahne single player 1. bölümü temsil ediyor
    }

    public void MultiplayerButton()
    {
        SceneManager.LoadScene(5); // burada 5. sahne lobby ekranını temsil ediyor(oyuncu beklediğimiz..)
    }
    public void QuitButton()
    {
        Debug.Log("QUIT!!");
        Application.Quit(); //build edildiğinde çalışır, oyunu kapatacak
    }

    public void ChooseCharacterButton()
    {
        SceneManager.LoadScene(10); 
    }

    public void GoMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public Text Volume;
    public Slider slider;

    private void Start()
    {
        LoadAudio();
    }

    public void SetAudio(float ses)
    {
        AudioListener.volume = ses;
        Volume.text = ((int)(ses * 100)).ToString(); // texte yazması için stringe çevirdik
        SaveAudio();
    }
    private void SaveAudio()
    {
        PlayerPrefs.SetFloat("audVol", AudioListener.volume);
    }
    private void LoadAudio()
    {
        if (PlayerPrefs.HasKey("audVol"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("audVol");
            slider.value = PlayerPrefs.GetFloat("audVol");

        }
        else
        {
            PlayerPrefs.SetFloat("audVol", 0.5f);
            AudioListener.volume = PlayerPrefs.GetFloat("audVol");
            slider.value = PlayerPrefs.GetFloat("audVol");
        }
        
    }
    
}
