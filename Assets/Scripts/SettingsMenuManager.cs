using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SettingsMenuManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject settingsMenu;
    public GameObject mainMenu;
    public GameObject pauseMenu;

    [Header("Tabs")]
    public GameObject generalPanel;
    public GameObject preferencesPanel;
    public Image generalTabImage;
    public Image preferencesTabImage;
    public Color activeTabColour = Color.white;
    public Color inactiveTabColour = new Color(0.55f, 0.55f, 0.55f);

    [Header("Buttons to select")]
    public GameObject generalFirstSelected;
    public GameObject preferencesFirstSelected;
    public GameObject mainMenuSettingsButton;
    public GameObject pauseMenuSettingsButton;

    public bool IsOpen { get; private set; }
    public bool BlocksPauseInput => IsOpen || Time.frameCount == closedOnFrame;

    private bool openedFromPause = false;
    private int closedOnFrame = -1;

    void Start()
    {
        settingsMenu.SetActive(false);
    }

    void Update()
    {
        if (!IsOpen)
            return;

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            CloseSettings();

        if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
            CloseSettings();

        if (Gamepad.current != null && Gamepad.current.leftShoulder.wasPressedThisFrame)
            ShowGeneralTab();

        if (Gamepad.current != null && Gamepad.current.rightShoulder.wasPressedThisFrame)
            ShowPreferencesTab();
    }

    public void OpenFromMainMenu()
    {
        openedFromPause = false;
        mainMenu.SetActive(false);
        OpenSettings();
    }

    public void OpenFromPauseMenu()
    {
        openedFromPause = true;
        pauseMenu.SetActive(false);
        OpenSettings();
    }

    private void OpenSettings()
    {
        settingsMenu.SetActive(true);
        IsOpen = true;
        ShowGeneralTab();
    }

    public void CloseSettings()
    {
        Debug.Log("CloseSettings called");

        settingsMenu.SetActive(false);
        IsOpen = false;
        closedOnFrame = Time.frameCount;

        if (openedFromPause)
        {
            pauseMenu.SetActive(true);
            Select(pauseMenuSettingsButton);
        }
        else
        {
            mainMenu.SetActive(true);
            Select(mainMenuSettingsButton);
        }
    }

    public void ShowGeneralTab()
    {
        generalPanel.SetActive(true);
        preferencesPanel.SetActive(false);

        if (generalTabImage != null) generalTabImage.color = activeTabColour;
        if (preferencesTabImage != null) preferencesTabImage.color = inactiveTabColour;

        Select(generalFirstSelected);
    }

    public void ShowPreferencesTab()
    {
        generalPanel.SetActive(false);
        preferencesPanel.SetActive(true);

        if (generalTabImage != null) generalTabImage.color = inactiveTabColour;
        if (preferencesTabImage != null) preferencesTabImage.color = activeTabColour;

        Select(preferencesFirstSelected);
    }

    private void Select(GameObject target)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(target);
    }
}