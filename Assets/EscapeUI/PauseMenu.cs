using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject speedometerUI;
    public GameObject pauseMenuUI;
    public SettingsMenu settingsMenu;
    public PlayerReset playerReset;  // Referencia a PlayerReset

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

        // Reanuda los sonidos del coche
        CarAudioController[] audioControllers = FindObjectsOfType<CarAudioController>();
        foreach (CarAudioController controller in audioControllers)
        {
            controller.ResumeAudio();
        }
    }


    void Pause()
    {
        pauseMenuUI.SetActive(true);
        speedometerUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;

        // Pausa los sonidos del coche
        CarAudioController[] audioControllers = FindObjectsOfType<CarAudioController>();
        foreach (CarAudioController controller in audioControllers)
        {
            controller.PauseAudio();
        }
    }


    public void Restart()
    {
        playerReset.ResetPlayer();  // Reiniciar el coche y el temporizador
        Resume();
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsMenu.OpenSettingsMenu();
    }
}
