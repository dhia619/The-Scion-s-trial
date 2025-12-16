using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField]
    private MusicLibrary soundLibrary;
    [SerializeField]
    private AudioSource soundSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    IEnumerator AnimateSoundCrossFade(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        if (soundSource == null || nextTrack == null) yield break;

        float percent = 0;
        
        // Fade out current track
        float startVolume = soundSource.volume;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            soundSource.volume = Mathf.Lerp(startVolume, 0, percent);
            yield return null;
        }

        // Switch to new track
        soundSource.clip = nextTrack;
        soundSource.Play();
        
        // Fade in new track
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            soundSource.volume = Mathf.Lerp(0, startVolume, percent); // Fixed: fade IN from 0 to startVolume
            yield return null;
        }
    }
    
    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        if (soundLibrary == null)
        {
            Debug.LogError("SoundLibrary not assigned in SoundManager");
            return;
        }
        
        AudioClip clip = soundLibrary.GetClipFromName(trackName);
        if (clip == null)
        {
            Debug.LogError($"Track '{trackName}' not found in SoundLibrary");
            return;
        }
        
        StartCoroutine(AnimateSoundCrossFade(clip, fadeDuration));
    }
}