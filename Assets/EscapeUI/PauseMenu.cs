using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject speedometerUI;
    public GameObject pauseMenuUI;
    public PlayerReset playerReset;
    public SettingsMenu settingsMenu;  // Reference to the SettingsMenu

    void Start()
    {
        Resume();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        speedometerUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        speedometerUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Restart()
    {
        playerReset.ResetPlayer();
        Resume();
    }

    public void OpenSettings()  
    {
        pauseMenuUI.SetActive(false);
        settingsMenu.OpenSettingsMenu();  
    }
}
