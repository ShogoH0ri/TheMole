using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour
{
    public static OptionMenu instance;

    public GameObject optionmenu;
    public GameObject mainmenu;
    public InputActionProperty showButton;
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        LoadVolume();

        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSoundVolume);
    }
    public void Back()
    {
        mainmenu.SetActive(true);
        optionmenu.SetActive(false);
        PlayerPrefs.Save();
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }


    public void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume",0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume",0.5f);

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        UpdateMusicVolume(musicVolume);
        UpdateSoundVolume(sfxVolume);
    }
}
