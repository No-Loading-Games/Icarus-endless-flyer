using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraViewportManager : MonoBehaviour
{
    public static CameraViewportManager Instance;

    [Header("Reference Resolution")]
    public float referenceWidth = 468f;
    public float referenceHeight = 988f;
    public float defaultOrthoSize = 4.94f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persist across scenes
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        AdjustMainCamera();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AdjustMainCamera();
    }

    public void AdjustMainCamera()
    {
        Camera cam = Camera.main;
        if (cam == null || !cam.orthographic) return;

        float screenRatio = (float)Screen.width / Screen.height;
        float referenceRatio = referenceWidth / referenceHeight;

        // Adjust orthographic size
        float orthoSize = defaultOrthoSize;
        if (screenRatio < referenceRatio)
        {
            // Taller than reference (crop top)
            orthoSize = defaultOrthoSize;
        }
        else
        {
            // Wider than reference (zoom in so width touches)
            orthoSize = defaultOrthoSize * (screenRatio / referenceRatio);
        }

        cam.orthographicSize = orthoSize;

        // Adjust vertical position for bottom anchoring
        float centerOffset = (orthoSize - defaultOrthoSize);
        cam.transform.position = new Vector3(
            cam.transform.position.x,
            centerOffset,
            cam.transform.position.z
        );

        // Optionally, adjust viewport rect if you're using it too
        // cam.rect = new Rect(...); 
    }
}
