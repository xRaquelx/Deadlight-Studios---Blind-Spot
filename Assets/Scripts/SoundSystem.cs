using System;
using UnityEngine;

public static class SoundSystem
{
    public static event Action<Vector3, float> OnSoundMade;

    // position where sound is made,, radius for max distance it can be heard
    // OnSoundMade is an event that notifies the enemy listening for sounds.
    public static void MakeSound(Vector3 position, float radius)
    {
        OnSoundMade?.Invoke(position, radius);
    }
}
