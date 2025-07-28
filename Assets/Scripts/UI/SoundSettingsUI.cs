using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsUI : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;
    public Toggle musicButton, sfxButton;

    public Sprite musicOn, musicOff, sfxOn, sfxOff;

    private float _musicVolume, _sfxVolume;

    /*private void Update()
    {
        _musicVolume = musicSlider.value;
        _sfxVolume = sfxSlider.value;
    }*/

    private void Awake()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            MusicVolume();
            SFXVolume();
        }
            
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
        if (musicButton.isOn)
        {
            musicButton.image.sprite = musicOn;
            musicSlider.value = _musicVolume;
            AudioManager.Instance.MusicVolume(musicSlider.value);
        }
        else
        {
            musicButton.image.sprite = musicOff;
            musicSlider.value = 0;
            AudioManager.Instance.MusicVolume(musicSlider.value);
            PlayerPrefs.SetFloat("musicVolume", 0);
        }
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
        if (sfxButton.isOn)
        {
            sfxButton.image.sprite = sfxOn;
            sfxSlider.value = _sfxVolume;
            AudioManager.Instance.SFXVolume(sfxSlider.value);
        }
        else
        {
            sfxButton.image.sprite = sfxOff;
            sfxSlider.value = 0;
            AudioManager.Instance.SFXVolume(sfxSlider.value);
            PlayerPrefs.SetFloat("sfxVolume", 0);
        }

    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
        if (musicSlider.value != 0)
        {
            //musicButton.interactable = true;
            _musicVolume = musicSlider.value;
            musicButton.isOn = true;
        }
        else
        {
            //ToggleMusic(); 
            //musicButton.interactable = false;
            musicButton.isOn = false;
        }
        PlayerPrefs.SetFloat("musicVolume", _musicVolume);
    }

    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(sfxSlider.value); 
        if (sfxSlider.value != 0)
        {
            //sfxButton.interactable = true;
            sfxButton.isOn = true;
            _sfxVolume = sfxSlider.value;
        }
        else
        {
            //ToggleSFX();
            //sfxButton.interactable = false;
            sfxButton.isOn = false;
        }
        PlayerPrefs.SetFloat("sfxVolume", _sfxVolume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
    }
}
