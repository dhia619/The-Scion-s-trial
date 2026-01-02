using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;


    private void Start()
    {
        Time.timeScale = 1;
        MusicManager.Instance.PlayMusic("MainMenu");
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;

        if (CheckPointManager.Instance != null && CheckPointManager.Instance.HasCheckpoint())
        {
            Debug.Log("[MainMenu] Continuing from checkpoint");
            LevelManager.Instance.LoadScene("SampleScene", "CrossFade");
        }
        else
        {
            LevelManager.Instance.LoadScene("IntroCutscene", "CrossFade");
        }
    }

    public void NewGame()
    {
        CheckPointManager.Instance?.DeleteCheckpoint();

        LevelManager.Instance.LoadScene("IntroCutscene", "CrossFade");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdatMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdatSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}
