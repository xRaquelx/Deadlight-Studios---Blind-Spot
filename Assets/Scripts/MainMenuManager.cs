using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public Camera menuCamera;
    public Camera playerCamera;
    public PlayerMovement playerMovement;

    void Start()
    {
        mainMenu.SetActive(true);

        menuCamera.enabled = true;
        playerCamera.enabled = false;

        playerMovement.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        mainMenu.SetActive(false);

        menuCamera.enabled = false;
        playerCamera.enabled = true;

        playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked!");
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
