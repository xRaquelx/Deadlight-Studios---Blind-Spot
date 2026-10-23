using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSoundTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        if (Keyboard.current == null)
        {
            Debug.LogWarning("Keyboard not detected!");
            return;
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Debug.Log("1 PRESSED");
            SoundSystem.MakeSound(transform.position, 5f);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Debug.Log("2 PRESSED");
            SoundSystem.MakeSound(transform.position, 15f);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            Debug.Log("3 PRESSED");
            SoundSystem.MakeSound(transform.position, 20f);
        }
    }

    public void MakeQuietNoise()
    {
        SoundSystem.MakeSound(transform.position, 5f);
    }

    public void MakeMediumNoise()
    {
        SoundSystem.MakeSound(transform.position, 15f);
    }

    public void MakeLoudNoise()
    {
        SoundSystem.MakeSound(transform.position, 20f);
    }
}
