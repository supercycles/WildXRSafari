using System.Collections;
using UnityEngine;

//Lock specific UI elements to match the horizontal rotation of the VR camera. Separate from LockSubtitles and LockYAxisFollow due to minor altercations
//Made with help of ChatGPT
public class LockUI : MonoBehaviour
{
    public Transform vrCamera;  // Reference to the VR camera
    public float fixedDistance = 0.5f;  // Distance to place the sphere in front of the camera
    private float fixedYPosition;  // Variable to store the fixed y-position
    private bool yAxisInitialized = false;  // To check if the camera's y position is initialized
    public float initializationDelay = 1.0f;  // Delay before initializing the y-axis
    public float yThreshold = .1f;  // Small threshold to account for minor differences in y positions

    void Start()
    {
        vrCamera = Camera.main.transform;
        StartCoroutine(InitializeYAxis());
    }

    IEnumerator InitializeYAxis()
    {
        // Wait for the initialization delay
        yield return new WaitForSeconds(initializationDelay);

        // After the delay, lock the y-axis based on the camera's current y position
        fixedYPosition = vrCamera.position.y;
        yAxisInitialized = true;
    }

    void Update()
    {
        if (!yAxisInitialized)
        {
            // Exit early if the y-axis is not yet initialized
            return;
        }

        // Get the forward direction of the camera but lock the y-axis so it stays horizontal
        Vector3 cameraForward = vrCamera.forward;
        cameraForward.y = 0;  // Lock the y-axis to keep horizontal movement only
        cameraForward.Normalize();  // Normalize to maintain consistent direction

        // Calculate the target position by keeping a fixed distance in front of the camera
        Vector3 targetPosition = vrCamera.position + cameraForward * fixedDistance;

        // Calculate the UI y position based on camera elevation
        // Maintain the initial fixed y-position when the camera elevation changes
        targetPosition.y = vrCamera.position.y;  // Track camera's y position

        // Update the UI position
        transform.position = targetPosition;

        // Optionally, make the UI face the camera (if rotation is desired)
        Vector3 lookDirection = vrCamera.position - transform.position;  // UI looks at the camera
        lookDirection.y = 0;  // Lock the y-axis rotation
        transform.rotation = Quaternion.LookRotation(lookDirection);
    }
}
