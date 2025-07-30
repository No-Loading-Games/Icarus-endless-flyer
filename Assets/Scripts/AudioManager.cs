using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public Sound wingFlapSFX;
    public Sound divingSFX;
    public AudioSource musicHandler, sfxHandler, sfxHandler2, burningSfxHandler, wingFlapSFXHandler;

    public AudioClip burningSFX;

    public AudioClip idleMusic;
    public AudioClip flyingMusic;

    private float _noGainedCoinMaxDuration = 1f;

    private int _diveSFXchecker = 0;

    [SerializeField]
    private float _coinPitch = .8f;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Audio manager started");
        PlayMusic("Idle");
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadVolume();
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("no music");
        }
        else
        {
            //Debug.Log("music playing");
            musicHandler.clip = s.clip;
            musicHandler.Play();
        }
    }

    public void PlaySFX(string name, float delay)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        float timer = 0f;

        while (timer < delay)
        {
            timer += Time.deltaTime;
            //Debug.Log("SFX timer: " + timer);
        }

        if (s == null)
        {
            Debug.Log("no Sfx");
        }
        else
        {
            sfxHandler.pitch = 1f;
            sfxHandler.PlayOneShot(s.clip);
        }
    }

    public void PlaySFX(string name, float minPitch, float maxPitch)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        //float timer = 0f;

        if (s == null)
        {
            Debug.Log("no Sfx");
        }
        else
        {
            sfxHandler2.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            sfxHandler2.PlayOneShot(s.clip);
        }
    }

    public void PlaySFX(string name, float delay, float pitchIncrement,float maxPitch)
    {
        StopAllCoroutines();
        StartCoroutine(NoGainedCoinTimer());

        Sound s = Array.Find(sfxSounds, x => x.name == name);

        float timer = 0f;

        while (timer < delay)
        {
            timer += Time.deltaTime;
            //Debug.Log("SFX timer: " + timer);
        }

        if (s == null)
        {
            Debug.Log("no Sfx");
        }
        else
        {
            if (_coinPitch <= maxPitch)
                _coinPitch += pitchIncrement;

            sfxHandler.pitch = _coinPitch;
            sfxHandler.PlayOneShot(s.clip);
        }
    }

    public void PlayBurningSFX(float fadeTimer, float targetPercentVolume)
    {

        AudioClip s = burningSFX;

        if (burningSfxHandler.volume <= 0.01f && targetPercentVolume == 0)
        {
            Debug.Log("VOLUME IS " + burningSfxHandler.volume);
            burningSfxHandler.volume = 0;
            return;
        }

        if (s == null)
        {
            Debug.Log("no music");
        }
        else
        {
            StartCoroutine(StartFade(burningSfxHandler, fadeTimer, targetPercentVolume));
            if (burningSfxHandler.isPlaying)
                return;

            Debug.Log("Burning SFX starting");
            burningSfxHandler.clip = s;
            burningSfxHandler.Play();
        }


    }

    public void PlayWingFlapSFX()
    {
        Sound s = wingFlapSFX;

        if (s == null)
        {
            Debug.Log("no Sfx");
        }
        else
        {
            wingFlapSFXHandler.pitch = 1f;
            wingFlapSFXHandler.PlayOneShot(s.clip);
        }
    }

    public void PlayDiveSFX(bool isDiving)
    {
        Sound s = divingSFX;

        if (s == null)
        {
            Debug.Log("No SFX");
            return;
        }

        if (isDiving)
        {
            if (_diveSFXchecker == 0)
            {
                sfxHandler.PlayOneShot(s.clip);
            }

            _diveSFXchecker++;

        }
        else if (isDiving == false)
            _diveSFXchecker = 0;

    }

    public void PauseAllsSFX()
    {
        sfxHandler.Pause();
        sfxHandler2.Pause();
        burningSfxHandler.Pause();
        wingFlapSFXHandler.Pause();
    }
    public void UnPauseAllsSFX()
    {
        sfxHandler.UnPause();
        sfxHandler2.UnPause();
        burningSfxHandler.UnPause();
        wingFlapSFXHandler.UnPause();
    }

    public void ToggleMusic()
    {
        musicHandler.mute = !musicHandler.mute;
        if (musicHandler.mute == true)
            musicHandler.volume = 0;
    }

    public void ToggleSFX()
    {
        sfxHandler.mute = !sfxHandler.mute;
        sfxHandler2.mute = !sfxHandler2.mute;
        burningSfxHandler.mute = !burningSfxHandler.mute;
        wingFlapSFXHandler.mute = !wingFlapSFXHandler.mute;
        if (sfxHandler.mute == true)
        {
            SFXVolume(0);
        }
    }

    public void MusicVolume(float volume)
    {
        musicHandler.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxHandler2.volume = (sfxHandler2.volume / sfxHandler.volume) * volume;
        burningSfxHandler.volume = (burningSfxHandler.volume / sfxHandler.volume) * volume;
        wingFlapSFXHandler.volume = (wingFlapSFXHandler.volume / sfxHandler.volume) * volume;
        sfxHandler.volume = volume;
    }

    private void LoadVolume()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            musicHandler.volume = PlayerPrefs.GetFloat("musicVolume");
            sfxHandler.volume = PlayerPrefs.GetFloat("sfxVolume");
            sfxHandler2.volume = PlayerPrefs.GetFloat("sfxVolume") * 0.4f;
            burningSfxHandler.volume = PlayerPrefs.GetFloat("sfxVolume");
            wingFlapSFXHandler.volume = PlayerPrefs.GetFloat("sfxVolume") * 0.1f;
        }
        else
        {
            musicHandler.volume = 1;
            sfxHandler.volume = 1;
            sfxHandler2.volume = 0.4f;
            burningSfxHandler.volume = 0;
            wingFlapSFXHandler.volume = 0.1f;
        }
    }

    private IEnumerator NoGainedCoinTimer()
    {
        float timer = 0;

        while(timer <= _noGainedCoinMaxDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        _coinPitch = .8f;
    }

    public IEnumerator StartFade(AudioSource audioSource, float duration, float percentVolume)
    {
        float targetVolume = sfxHandler.volume * percentVolume * 1.4f;
        float currentTime = 0;
        float start = audioSource.volume;


        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            Debug.Log("FADE TIMER: " + currentTime);
            yield return null;
        }

        Debug.Log("FADE TIMER DONE");

        /*if (targetVolume == 0 && AudioManager.Instance.burningSfxHandler.isPlaying)
        {
            AudioManager.Instance.burningSfxHandler.Stop();
        }*/

    }
}
