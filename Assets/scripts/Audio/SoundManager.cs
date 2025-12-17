using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    private AudioSource source;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public void PlayLoop(AudioClip clip)
    {
        if (source.clip == clip && source.isPlaying)
            return;

        source.clip = clip;
        source.loop = true;
        source.Play();
    }

    public void StopLoop()
    {
        source.Stop();
        source.clip = null;
    }

    public bool IsPlaying(AudioClip clip)
    {
        return source.clip == clip && source.isPlaying;
    }
}
