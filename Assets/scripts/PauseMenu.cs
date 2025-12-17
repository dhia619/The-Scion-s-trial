using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
     [SerializeField] GameObject pauseMenu; 
    public void Pause()
    {
        pauseMenu.SetActive(true);
        // Time.timeScale = 0;
    }

    public void Home()
    {
        // Time.timeScale = 1; 
        pauseMenu.SetActive(false);
        LevelManager.Instance.LoadScene("MainMenu", "CrossFade");
    }


    public void Resume()
    {
        pauseMenu.SetActive(false);
        // Time.timeScale = 1;

    }
}
