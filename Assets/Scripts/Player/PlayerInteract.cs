using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float interactionDistance;

    [SerializeField] private InputActionReference interactionReference;

    private void OnEnable()
    {
        // enabling the input action reference
        interactionReference.action.Enable();
        // subscribes to an event, which is called when the button is pressed
        interactionReference.action.started += PlayerInteracted;
    }

    private void OnDisable()
    {
        interactionReference.action.started -= PlayerInteracted;
        interactionReference.action.Disable();
    }

    private void PlayerInteracted(InputAction.CallbackContext context)
    {
        // if button was clicked
        if (context.started)
        {
            // get ray from camera
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);

            // if the ray doesnt hit anything within the distance
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, interactionDistance)) return;

            // check if the object is interactable by interface
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactableObject))
            {
                interactableObject.Interact();
            }
        }
    }
}
