using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    [SerializeField] private RawImage cubePhoto;
    [SerializeField] private RawImage spherePhoto;
    [SerializeField] private RawImage cylinderPhoto;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
