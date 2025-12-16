using UnityEngine;
using UnityEngine.Audio;


public class SampleScene : MonoBehaviour
{
    public AudioMixer audioMixer;
    private void Start()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayMusic("GameMusic");
        }
        
        Debug.Log("SampleScene loaded!");
    }


    public void UpdatMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdatSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }
    
    public void ReturnToMainMenu()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayMusic("MainMenu");
        }
        
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.LoadScene("MainMenu", "CrossFade");
        }
        // else
        // {
        //     SceneManager.LoadScene("MainMenu");
        // }
    }
}