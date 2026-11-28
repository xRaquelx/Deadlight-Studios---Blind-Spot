using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public Camera menuCamera;
    public Camera playerCamera;
    public PlayerMovement playerMovement;
    public GameObject firstSelectedButton;

    public bool IsInMainMenu { get; private set; }

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        IsInMainMenu = true;
        mainMenu.SetActive(true);

        menuCamera.enabled = true;
        playerCamera.enabled = false;

        playerMovement.enabled = false;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        IsInMainMenu = false;

        mainMenu.SetActive(false);

        menuCamera.enabled = false;
        playerCamera.enabled = true;

        playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {     
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
