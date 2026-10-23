using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;

    [Header("Looking")]
    public Transform cameraTransform;
    public float mouseSensitivity = 0.1f;
    public float stickSensitivity = 150f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    private CharacterController controller;
    private float pitch;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    void HandleLook()
    {
        Vector2 look = Vector2.zero;

        // Mouse: delta is already per-frame, so no deltaTime
        if (Mouse.current != null)
            look += Mouse.current.delta.ReadValue() * mouseSensitivity;

        // Right stick: it's a rate, so it needs deltaTime
        //if (Gamepad.current != null)
        //{
        //    Vector2 stick = Gamepad.current.rightStick.ReadValue();
        //    look += stick * stickSensitivity * Time.deltaTime;
        //}

        // Left/right turns the whole body
        transform.Rotate(Vector3.up * look.x);

        // Up/down tilts only the camera
        pitch = Mathf.Clamp(pitch - look.y, minPitch, maxPitch);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void HandleMovement()
    {
        Vector2 input = Vector2.zero;

        // Keyboard controls
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)
                input.y -= 1f;

            if (Keyboard.current.aKey.isPressed)
                input.x += 1f;

            if (Keyboard.current.sKey.isPressed)
                input.y += 1f;

            if (Keyboard.current.dKey.isPressed)
                input.x -= 1f;
        }

        // Controller controls
        //if (Gamepad.current != null)
        //{
        //    Vector2 controllerInput = Gamepad.current.leftStick.ReadValue();

        //    // D-pad support
        //    if (Gamepad.current.dpad.left.isPressed)
        //        controllerInput.x -= 1f;

        //    if (Gamepad.current.dpad.right.isPressed)
        //        controllerInput.x += 1f;

        //    if (Gamepad.current.dpad.down.isPressed)
        //        controllerInput.y -= 1f;

        //    if (Gamepad.current.dpad.up.isPressed)
        //        controllerInput.y += 1f;

        //    // Prefer controller input when it is being used
        //    if (controllerInput.sqrMagnitude > 0.01f)
        //        input = controllerInput;
        //}

        input = Vector2.ClampMagnitude(input, 1f);

        Vector3 movement = new Vector3(input.x, 0f, input.y);

        controller.Move(movement * moveSpeed * Time.deltaTime);
    }
}