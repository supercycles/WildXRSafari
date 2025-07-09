using System.Collections;
using UnityEngine;

//Lock specific UI elements to match the horizontal rotation of the VR camera. Separate from LockSubtitles and LockUI due to minor altercations
//This script in particular is made for the "digital camera" to always follow the players hip
//Made with help of ChatGPT
public class LockYAxisFollow : MonoBehaviour
{
    public Transform vrCamera;  // Reference to the VR camera
    public float fixedDistance = 0.5f;  // Distance to place the object in front of the camera
    public float horizontalOffset = 0.0f;  // Offset for left-right movement
    public float verticalOffset = 0.0f;  // Offset for up-down movement
    private float fixedYPosition;  // Variable to store the fixed y-position
    private bool yAxisInitialized = false;  // To check if the camera's y position is initialized
    public float initializationDelay = 1.0f;  // Delay before initializing the y-axis

    private Quaternion initialRotation;
    private bool isBeingGrabbed = false;  // Flag to check if the object is being grabbed

    void Start()
    {
        initialRotation = transform.rotation;
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
        if (isBeingGrabbed)
        {
            // Do nothing if the object is being grabbed
            return;
        }

        if (!yAxisInitialized)
        {
            // Exit early if the y-axis is not yet initialized
            return;
        }

        // Calculate the target position by keeping a fixed distance in front of the camera
        Vector3 targetPosition = vrCamera.position + vrCamera.forward * fixedDistance + vrCamera.right * horizontalOffset;
        targetPosition.y = vrCamera.position.y + verticalOffset;

        // Update the object's position
        transform.position = targetPosition;

        // Calculate the rotation to face the camera
        Vector3 lookDirection = vrCamera.position - transform.position;  // Object looks at the camera
        lookDirection.y = 0;  // Lock the y-axis rotation
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = targetRotation * Quaternion.Inverse(Quaternion.Euler(initialRotation.eulerAngles));
    }

    // Function to call when the object is picked up
    public void OnGrab()
    {
        isBeingGrabbed = true;
        StopCoroutine("UpdatePosition");  // Stop the coroutine if it's running
    }

    // Function to call when the object is released
    public void OnRelease()
    {
        isBeingGrabbed = false;
        StartCoroutine("UpdatePosition");  // Restart the coroutine to update the position
    }

    // Coroutine to handle object position when not being grabbed
    IEnumerator UpdatePosition()
    {
        while (!isBeingGrabbed)
        {
            if (yAxisInitialized)
            {
                Vector3 targetPosition = vrCamera.position + vrCamera.forward * fixedDistance + vrCamera.right * horizontalOffset;
                targetPosition.y = vrCamera.position.y + verticalOffset;

                transform.position = targetPosition;

                Vector3 lookDirection = vrCamera.position - transform.position;
                lookDirection.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = targetRotation * Quaternion.Inverse(Quaternion.Euler(initialRotation.eulerAngles));
            }

            yield return null;  // Wait until the next frame
        }
    }
}