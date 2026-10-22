using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
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