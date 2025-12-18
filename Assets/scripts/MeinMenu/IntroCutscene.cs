using UnityEngine;
using UnityEngine.Video;

public class IntroCutscene : MonoBehaviour
{
    public string nextSceneName;
    public KeyCode skipKey = KeyCode.Space;

    private VideoPlayer videoPlayer;
    private bool hasSkipped = false;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        if (!hasSkipped && Input.GetKeyDown(skipKey))
        {
            SkipCutscene();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SkipCutscene();
    }

    void SkipCutscene()
    {
        if (hasSkipped) return;

        hasSkipped = true;

        videoPlayer.Stop();
        LevelManager.Instance.LoadScene(nextSceneName);
    }
}
