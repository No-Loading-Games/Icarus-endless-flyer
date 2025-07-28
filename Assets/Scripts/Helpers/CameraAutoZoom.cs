using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraAutoZoom : MonoBehaviour
{

    public float referenceWidth = 468f;
    public float referenceHeight = 988f;
    public float referenceOrthoSize = 4.94f;

    public Camera cam;

    void Start()
    {
        //cam = GetComponent<Camera>();
        UpdateCameraSettings();
    }

    void UpdateCameraSettings()
    {
        float targetAspect = referenceWidth / referenceHeight;
        float screenAspect = (float)Screen.width / Screen.height;

        // Match width, adjust ortho size accordingly
        cam.orthographicSize = referenceOrthoSize * (referenceWidth / Screen.width * (Screen.height / referenceHeight));

        // Calculate height in world units
        float targetHeightWorld = cam.orthographicSize * 2;
        float refHeightWorld = referenceOrthoSize * 2;

        // Crop vertical by adjusting viewport
        float heightRatio = refHeightWorld / targetHeightWorld;
        if (heightRatio < 1f)
        {
            // Camera sees more vertically — crop top
            float cropAmount = 1f - heightRatio;
            cam.rect = new Rect(0f, 0f, 1f, heightRatio); // anchor to bottom
        }
        else
        {
            // No cropping needed
            cam.rect = new Rect(0f, 0f, 1f, 1f);
        }
        Debug.Log("Cam position: " + cam.transform.position);
        // Force camera to anchor from bottom
        float yOffset = cam.orthographicSize - referenceOrthoSize;
        cam.transform.position = new Vector3(cam.transform.position.x, yOffset, cam.transform.position.z);
        Debug.Log("Cam NEW position: " + cam.transform.position);

    }

    /*void Awake()
    {
        //Debug.Log("Cam position: " + cam.transform.position);
        // Force camera to anchor from bottom
        float yOffset = cam.orthographicSize - referenceOrthoSize;
        cam.transform.position = new Vector3(cam.transform.position.x, yOffset, cam.transform.position.z);
        //Debug.Log("Cam NEW position: " + cam.transform.position);
    }*/

    private void Update()
    {
        UpdateCameraSettings();
    }

}
