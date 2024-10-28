using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenuUI;
    public GameObject controlsMenuUI;
    public GameObject volumeMenuUI;

    public GameObject pauseMenuUI;  

    void Start()
    {
        settingsMenuUI.SetActive(false);
        controlsMenuUI.SetActive(false);
        volumeMenuUI.SetActive(false);
    }

    public void OpenSettingsMenu()
    {
        settingsMenuUI.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsMenuUI.SetActive(false);
    }

    public void OpenControlsMenu()
    {
        controlsMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }

    public void OpenVolumeMenu()
    {
        volumeMenuUI.SetActive(true);
        CloseSettingsMenu();
    }

    public void BackToSettings()
    {
        controlsMenuUI.SetActive(false);
        volumeMenuUI.SetActive(false);
        OpenSettingsMenu();
    }

    public void BackToPauseMenu()  
    {
        
        pauseMenuUI.SetActive(true);
        CloseSettingsMenu();
    }
}
