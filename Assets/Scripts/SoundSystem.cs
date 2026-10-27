using System;
using UnityEngine;

public enum SoundPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
}

public static class SoundSystem
{
    public static event Action<Vector3, SoundPriority> OnSoundMade;

    // position where sound is made,, radius for max distance it can be heard
    // OnSoundMade is an event that notifies the enemy listening for sounds.
    public static void MakeSound(Vector3 position, SoundPriority priority)
    {
        OnSoundMade?.Invoke(position, priority);
    }
}
