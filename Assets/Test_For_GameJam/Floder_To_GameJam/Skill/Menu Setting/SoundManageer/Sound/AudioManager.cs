using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic("Start_Game");
    }
    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == name);
        if(sound != null)
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }else
        {
            Debug.Log("No Sound");
        }
    }
    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == name);
        if (sound != null)
        {
            sfxSource.clip = sound.clip;
            sfxSource.PlayOneShot(sound.clip);
        }
        else
        {
            Debug.Log("No Sound");
        }
    }
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
