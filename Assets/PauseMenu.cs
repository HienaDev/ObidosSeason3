using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private PauseMenuBackgroundAnimation backgroundAnim;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject gameUI; // in-game UI
    [SerializeField] private MainMenuState mainMenuState; // main menu manager
    [SerializeField]
    private GameObject optionsMenu;
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private GameObject Instructions;
    [SerializeField]
    private GameObject InstructionsReturn;
    [SerializeField]
    private GameObject confirmationPopup;
    [SerializeField]
    private AudioClip hitClip;

    private PlayerMovement playerMov;

    private bool isPaused = false;

    private void Start()
    {
        playerMov = FindAnyObjectByType<PlayerMovement>();
    }

    void Update()
    {
        if (levelManager != null && levelManager.isRunning)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    OpenOptions(false);
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }

    public void PauseGame()
    {
        playerMov.CanPlaySound = false;
        isPaused = true;
        Time.timeScale = 0f;
        backgroundAnim.OpenBackground();

        // hide in-game UI while paused
        if (gameUI != null)
        {
            gameUI.SetActive(false);
        }
    }

    public void OpenOptions(bool open)
    {
        optionsMenu.SetActive(open);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        backgroundAnim.CloseBackground();

        playerMov.CanPlaySound = true;

        // restore in-game UI
        if (gameUI != null)
        {
            gameUI.SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
        ResumeGame(); // ensure unpaused
        SceneManager.LoadScene(0);

        /*if (levelManager != null)
        {
            levelManager.ResetToMainMenu(); // stop level, deactivate isRunning, reset all
        }

        if (mainMenuState != null)
        {
            mainMenuState.RestoreState(); // reactivate main menu civillians
        }

        if (gameUI != null)
            gameUI.SetActive(false); // deactivate player UI*/
    }

    public void OpenConfirmationPopup(bool open)
    {
        confirmationPopup.SetActive(open);
    }

    public void ChangeMusicVolume()
    {
        if (musicSlider.value > -44)
        {
            audioMixer.SetFloat("MusicVolume", musicSlider.value);
        }
        else
        {
            audioMixer.SetFloat("MusicVolume", -80);
        }

    }

    public void ChangeSFXVolume()
    {
        if (sfxSlider.value > -44)
        {
            audioMixer.SetFloat("SFXVolume", sfxSlider.value);
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", -80);
        }

    }

    public void OpenInstructions()
    {
        Instructions.SetActive(true);
        InstructionsReturn.SetActive(true);
    }

    public void PlayHitSFX()
    {
        playerMov.gameObject.GetComponent<AudioSource>().PlayOneShot(hitClip);
    }
}
