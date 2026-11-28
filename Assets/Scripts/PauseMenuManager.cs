using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public MainMenuManager mainMenuManager;

    public SettingsMenuManager settingsMenuManager;

    public PlayerMovement playerMovement;

    public Camera menuCamera;
    public Camera playerCamera;

    public GameObject firstSelectedButton;

    private bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (mainMenuManager.IsInMainMenu || settingsMenuManager.BlocksPauseInput)
            return;

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);

        Time.timeScale = 0f;

        playerMovement.enabled = false;

        EventSystem.current.SetSelectedGameObject(firstSelectedButton);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);

        Time.timeScale = 1f;

        playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReturnToMainMenu()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);

        mainMenuManager.ShowMainMenu();
    }
}
