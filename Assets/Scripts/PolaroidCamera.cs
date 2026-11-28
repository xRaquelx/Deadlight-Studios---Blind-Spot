using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PolaroidCamera : MonoBehaviour
{
    // in movement script, we can call this to decide when players should move slower 
    public bool cameraOut = false;

    [Header("Action References")]
    [SerializeField] private InputActionReference cameraReference;
    [SerializeField] private InputActionReference pictureReference;

    [Header("Other References")]
    [SerializeField] private Transform playerCameraPos;
    [SerializeField] private Journal journalScript;

    [Header("Polaoid Camera")]
    [SerializeField] private GameObject cameraOverlay;
    [SerializeField] private Camera polaroidCamera;
    [SerializeField] private RenderTexture photoRenderTexture;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraOverlay.SetActive(false);
    }

    private void OnEnable()
    {
        // enabling the input action reference
        cameraReference.action.Enable();
        // subscribes to an event, which is called when the button is pressed
        cameraReference.action.started += ToggleCamera;

        pictureReference.action.Enable();
        pictureReference.action.started += TakePicture;
    }

    private void OnDisable()
    {
        cameraReference.action.started -= ToggleCamera;
        cameraReference.action.Disable();

        pictureReference.action.started -= TakePicture;
        pictureReference.action.Disable();
    }

    private void ToggleCamera(InputAction.CallbackContext context)
    {
        if (journalScript.journalOpen) return;

        // if the camera is out, make it not out
        cameraOut = !cameraOut;
        cameraOverlay.SetActive(cameraOut);
        Debug.Log("Camera toggled?: " + cameraOut);

        // animation of pulling it out/putting away
    }

    private void TakePicture(InputAction.CallbackContext context)
    {
        if (!cameraOut) return;
        if (journalScript.journalOpen) return;

        Ray ray = new Ray(playerCameraPos.position, playerCameraPos.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            GameObject photographedObject = hitInfo.collider.gameObject;

            Debug.Log("Took picture");
            Texture2D photo = CapturePhoto();

            journalScript.OpenJournal();
            cameraOut = false;
            cameraOverlay.SetActive(false);

            journalScript.AddPhoto(photographedObject.tag, photo);
        }
    }

    private Texture2D CapturePhoto()
    {
        // render the cams current view
        polaroidCamera.Render();

        // save currently active render text
        RenderTexture previousActive = RenderTexture.active;
        // select photo text to read pixel frm
        RenderTexture.active = photoRenderTexture;

        //create img with same res as render text
        Texture2D photo = new Texture2D(photoRenderTexture.width, photoRenderTexture.height, TextureFormat.RGB24, false);
        // copy render text pixels to img
        photo.ReadPixels(new Rect(0, 0, photoRenderTexture.width, photoRenderTexture.height), 0, 0);

        // apply copied pixels
        photo.Apply();

        // restore prev settings
        RenderTexture.active = previousActive;

        return photo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

