using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnvironmentalSounds : MonoBehaviour
{
    [Header("Timing Settings")]
    public float minDelay = 10f;
    public float maxDelay = 20f;

    public List<Transform> targets;
    public int currentTargetIdx;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //currentTargetIdx = Random.Range(0, targets.Count);

        //StartCoroutine(PlayAtRandomTimes());
    }

    //IEnumerator PlayAtRandomTimes()
    //{
    //    while (true)
    //    {
    //        float waitTime = Random.Range(minDelay, maxDelay);
    //        yield return new WaitForSeconds(waitTime);

    //        if (targets == null || targets.Count <= 1) yield break;

    //        int randomIdx;

    //        do
    //        {
    //            randomIdx = Random.Range(0, targets.Count);
    //        }
    //        while (randomIdx == currentTargetIdx);

    //        currentTargetIdx = randomIdx;

    //        Transform targetPosition = targets[randomIdx];

    //        SoundSystem.MakeSound(targetPosition.position, 60f, SoundPriority.Low);
    //    }
    //}

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
            SoundSystem.MakeSound(transform.position, 40f, SoundPriority.Low);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Debug.Log("2 PRESSED");
            SoundSystem.MakeSound(transform.position, 50f, SoundPriority.Medium);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            Debug.Log("3 PRESSED");
            SoundSystem.MakeSound(transform.position, 60f, SoundPriority.High);
        }
    }
}
