using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    // in movement script, we can call this to decide when players should move slower 
    public bool cameraOut = false;

    [Header("Action References")]
    [SerializeField] private InputActionReference cameraReference;
    [SerializeField] private InputActionReference pictureReference;

    [Header("Other References")]
    [SerializeField] private Transform playerCameraPos;
    [SerializeField] private Journal journal;

    [Header("Polaoid Camera")]
    [SerializeField] private GameObject cameraOverlay;
    [SerializeField] private Camera polaroidCamera;
    [SerializeField] private RenderTexture photoRenderTexture;


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
        // if the camera is out, make it not out
        cameraOut = !cameraOut;
        cameraOverlay.SetActive(cameraOut);
        Debug.Log("Camera toggled?: " + cameraOut);

        // animation of pulling it out/putting away
    }

    private void TakePicture(InputAction.CallbackContext context)
    {
        if (!cameraOut) return;

        Ray ray = new Ray(playerCameraPos.position, playerCameraPos.forward);

        // if the ray doesnt hit anything within the distance
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            GameObject photographedObject = hitInfo.collider.gameObject;

            Debug.Log("Took picture");
            Texture2D photo = CapturePhoto();
            journal.AddPhoto(photographedObject.tag, photo);
        }
    }

    private Texture2D CapturePhoto()
    {
        // store the cams original render target
        //RenderTexture previousTexture = playerCamera.targetTexture;
        // make the camera render into our photo texture
        //playerCamera.targetTexture = photoRenderTexture;

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
        //playerCamera.targetTexture = previousTexture;

        return photo;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



//   add button to open and close

//   click button to take picture
//   will check what the tag is of the object
//   sends it to the journal