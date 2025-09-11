using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainmenu;
    public GameObject option;
    public InputActionProperty showButton;
    public AudioMixer audioMixer;

    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Main Menu Buttons")]
    public Button optionButton;
    public Button quitButton;
    void Start()
    {
        LoadVolume();
        EnableMainMenu();

        optionButton.onClick.AddListener(EnableOption);
        quitButton.onClick.AddListener(QuitGame);

        MusicManager.Instance.PlayMusic("MainMenu");

        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSoundVolume);
    }
    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
        MusicManager.Instance.PlayMusic("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
                  Application.Quit();//ゲームプレイ終了
#endif
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        UpdateMusicVolume(musicVolume);
        UpdateSoundVolume(sfxVolume);
    }

    public void EnableOption()
    {
        mainmenu.SetActive(false);
        option.SetActive(true);
    }
    public void EnableMainMenu()
    {
        mainmenu.SetActive(true);
        option.SetActive(false);
    }

}
