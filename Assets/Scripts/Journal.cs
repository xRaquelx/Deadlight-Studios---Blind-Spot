using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    public bool journalOpen;
    [SerializeField] private GameObject journal;
    [SerializeField] private InputActionReference journalReference;

    [SerializeField] private PolaroidCamera polaroidCamera;

    [SerializeField] private RawImage cubePhoto;
    [SerializeField] private RawImage spherePhoto;
    [SerializeField] private RawImage cylinderPhoto;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        journal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        journalReference.action.Enable();
        journalReference.action.started += ToggleCamera;
    }

    private void OnDisable()
    {
        journalReference.action.started -= ToggleCamera;
        journalReference.action.Disable();
    }

    private void ToggleCamera(InputAction.CallbackContext context)
    {
        if (polaroidCamera.cameraOut) return;

        journalOpen = !journalOpen;
        journal.SetActive(journalOpen);
    }

    public void AddPhoto(string photoName, Texture2D photo)
    {
        switch (photoName)
        {
            case "Cube":
                cubePhoto.texture = photo;
                break;
            case "Sphere":
                spherePhoto.texture = photo;
                break;
            case "Cylinder":
                cylinderPhoto.texture = photo;
                break;
            default:
                Debug.Log("No journal slot for: " + photoName);
                break;
        }
    }

    public void OpenJournal()
    {
        journal.SetActive(true);
        journalOpen = true;
    }
}
