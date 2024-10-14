using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject SoundMenu;
    public Slider _musicSlider, _sfxSlider;

    [Header("UI Music")]
    public GameObject OnMusic;
    public GameObject OffMusic;

    [Header("UI SFX")]
    public GameObject OnSFX;
    public GameObject OffSFX;
    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
        if (AudioManager.instance.musicSource.mute)
        {
            OnMusic.SetActive(false);
            OffMusic.SetActive(true);
        }
        else
        {
            OnMusic.SetActive(true);
            OffMusic.SetActive(false);
        }
    }
    public void ToggleSFX()
    {
        AudioManager.instance.ToggleSFX();
        if (AudioManager.instance.sfxSource.mute)
        {
            OnSFX.SetActive(false);
            OffSFX.SetActive(true);
        }
        else
        {
            OnSFX.SetActive(true);
            OffSFX.SetActive(false);
        }
    }
    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(_musicSlider.value);
    }
    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(_sfxSlider.value);
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenSoundMenu()
    {
        SoundMenu.SetActive(true);
    }
    public void CloseSoundMenu()
    {
        SoundMenu.SetActive(false);
    }
}
