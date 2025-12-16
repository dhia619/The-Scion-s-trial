using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Slider progressBar;
    public GameObject transitionsContainer;
 
    private SceneTransition[] transitions;
 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Initialize transitions here to ensure they're ready
            if (transitionsContainer != null)
                transitions = transitionsContainer.GetComponentsInChildren<SceneTransition>(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }
 
    private void Start()
    {
        // Hide progress bar at start
        if (progressBar != null)
            progressBar.gameObject.SetActive(false);
    }
 
    public void LoadScene(string sceneName, string transitionName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, transitionName));
    }
 
    private IEnumerator LoadSceneAsync(string sceneName, string transitionName)
    {
        // Find the transition - with error handling
        SceneTransition transition = FindTransition(transitionName);
        if (transition == null)
        {
            Debug.LogError($"Transition '{transitionName}' not found!");
            SceneManager.LoadScene(sceneName);
            yield break;
        }

        // Start transition IN
        yield return StartCoroutine(transition.AnimateTransitionIn());

        // Set up async loading
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        // Show and update progress bar
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
            progressBar.value = 0f;
        }

        // Load progress
        while (scene.progress < 0.9f)
        {
            if (progressBar != null)
                progressBar.value = scene.progress;
            yield return null;
        }

        // Final progress update
        if (progressBar != null)
            progressBar.value = 1f;

        // Optional: Wait a moment at full progress
        yield return new WaitForSeconds(0.5f);

        // Allow scene activation
        scene.allowSceneActivation = true;

        // Wait for scene to fully activate
        yield return new WaitForSeconds(0.1f);

        // Hide progress bar
        if (progressBar != null)
            progressBar.gameObject.SetActive(false);

        // Transition OUT
        yield return StartCoroutine(transition.AnimateTransitionOut());
    }

    private SceneTransition FindTransition(string transitionName)
    {
        if (transitions == null || transitions.Length == 0)
        {
            if (transitionsContainer != null)
                transitions = transitionsContainer.GetComponentsInChildren<SceneTransition>(true);
        }

        return transitions?.FirstOrDefault(t => t.name == transitionName);
    }

    // Overload without transition name (uses first available transition)
    public void LoadScene(string sceneName)
    {
        if (transitions != null && transitions.Length > 0)
        {
            LoadScene(sceneName, transitions[0].name);
        }
        else
        {
            StartCoroutine(LoadSceneSimple(sceneName));
        }
    }

    private IEnumerator LoadSceneSimple(string sceneName)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        yield return scene;
    }
}