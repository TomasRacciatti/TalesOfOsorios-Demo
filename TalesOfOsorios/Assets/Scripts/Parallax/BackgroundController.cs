using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform[] backgroundLayers;
    
    [Header("Parallax Settings")]
    [SerializeField] private float[] parallaxFactors;
    
    private Vector3 _lastCameraPosition;
    
    private void Start()
    {
        if (cameraTransform == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                cameraTransform = mainCam.transform;
            }
        }

        _lastCameraPosition = cameraTransform.position;

        // debugger
        ValidateArrayLengths();
    }
    
    private void FixedUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - _lastCameraPosition;

        for (int i = 0; i < backgroundLayers.Length; i++)
        {
            if (backgroundLayers[i] != null)
            {
                float parallaxOffset = deltaMovement.x * parallaxFactors[i];
                Vector3 newPosition = backgroundLayers[i].position;
                newPosition.x += parallaxOffset;
                backgroundLayers[i].position = newPosition;
            }
        }

        _lastCameraPosition = cameraTransform.position;
    }

    private void ValidateArrayLengths()
    {
        if (backgroundLayers.Length != parallaxFactors.Length)
        {
            Debug.LogWarning("Background layers and parallax factors arrays must be the same length!");
        }
    }
}
