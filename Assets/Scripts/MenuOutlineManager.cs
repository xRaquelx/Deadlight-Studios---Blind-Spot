
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuOutlineManager : MonoBehaviour
{
    private Outline[] allOutlines;
    private GameObject previousButton;

    void Start()
    {
        allOutlines = FindObjectsByType<Outline>(FindObjectsInactive.Include);

        foreach (Outline outline in allOutlines)
        {
            outline.enabled = false;
        }
    }

    void Update()
    {
        if (EventSystem.current == null)
            return;

        GameObject currentButton = EventSystem.current.currentSelectedGameObject;

        if (currentButton == previousButton)
            return;

        foreach (Outline outline in allOutlines)
        {
            outline.enabled = false;
        }

        if (currentButton != null)
        {
            Outline selectedOutline = currentButton.GetComponent<Outline>();

            if (selectedOutline != null)
                selectedOutline.enabled = true;
        }

        previousButton = currentButton;
    }
}
