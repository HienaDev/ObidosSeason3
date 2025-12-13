using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private PauseMenuBackgroundAnimation backgroundAnim;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject gameUI; // in-game UI
    [SerializeField] private MainMenuState mainMenuState; // main menu manager

    private bool isPaused = false;

    void Update()
    {
        if (levelManager != null && levelManager.isRunning)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        backgroundAnim.OpenBackground();

        // hide in-game UI while paused
        if (gameUI != null)
        {
            gameUI.SetActive(false);
        }
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        backgroundAnim.CloseBackground();

        // restore in-game UI
        if (gameUI != null)
        {
            gameUI.SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
        ResumeGame(); // ensure unpaused

        if (levelManager != null)
        {
            levelManager.ResetToMainMenu(); // stop level, deactivate isRunning, reset all
        }

        if (mainMenuState != null)
        {
            mainMenuState.RestoreState(); // reactivate main menu civillians
        }

        if (gameUI != null)
            gameUI.SetActive(false); // deactivate player UI
    }
}
