using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        // just to test
        Debug.Log("Interacting");
    }
}
