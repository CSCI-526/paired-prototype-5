using UnityEngine;

public class TwoPlayerCamera : MonoBehaviour
{
    // --- Settings in Inspector ---
    [Tooltip("The first player to follow")]
    public Transform player1;

    [Tooltip("The second player to follow")]
    public Transform player2;

    [Tooltip("The smooth speed of the camera's movement and zoom")]
    public float smoothSpeed = 0.1f;
    
    [Tooltip("The minimum orthographic size for the camera (when players are close)")]
    public float minSize = 5f;

    [Tooltip("The maximum orthographic size for the camera (when players are far apart)")]
    public float maxSize = 10f;

    [Tooltip("The buffer zone near the screen edge. 0.2 means a 20% buffer.")]
    [Range(0f, 0.5f)]
    public float screenEdgeBuffer = 0.2f;

    [Tooltip("The offset from the players on the Z axis")]
    public float zOffset = -10f;

    [Tooltip("An empty object marking the bottom-left boundary of the level")]
    public Transform minBounds;

    [Tooltip("An empty object marking the top-right boundary of the level")]
    public Transform maxBounds;

    // --- Private variables ---
    private Camera mainCamera;

    void Start()
    {
        // Get the Camera component reference
        mainCamera = GetComponent<Camera>();
        // Instantly set the camera's initial state to avoid a jump at the start of the game
        SetInitialCameraState();
    }

    void LateUpdate()
    {
        // Ensure all required transforms have been assigned
        if (player1 == null || player2 == null || minBounds == null || maxBounds == null)
        {
            return;
        }

        // Calculate and smoothly set the camera's size
        float targetSize = CalculateTargetSize();
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, smoothSpeed);

        // Calculate and smoothly set the camera's position, clamped within the bounds
        Vector3 targetPosition = CalculateTargetPosition();
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
    
    // --- NEW AND REFACTORED FUNCTIONS ---

    // Calculates the target orthographic size for the camera
    private float CalculateTargetSize()
    {
        // Calculate the distance between players on both axes
        float horizontalDistance = Mathf.Abs(player1.position.x - player2.position.x);
        float verticalDistance = Mathf.Abs(player1.position.y - player2.position.y);
        
        // Calculate the required size to fit players based on horizontal distance and screen aspect ratio
        float requiredSizeByWidth = horizontalDistance * Screen.height / Screen.width * 0.5f;
        // Calculate the required size to fit players based on vertical distance
        float requiredSizeByHeight = verticalDistance * 0.5f;
        
        // The final required size is the larger of the two, to ensure both axes are visible
        float requiredSize = Mathf.Max(requiredSizeByWidth, requiredSizeByHeight);

        // --- [CORE LOGIC CHANGE] ---
        // Artificially increase the required size to create a buffer zone
        // This makes the players occupy a smaller central portion of the screen
        float buffer = 1.0f - screenEdgeBuffer;
        if (buffer > 0.01f) // Avoid division by zero or a very small number
        {
            requiredSize /= buffer;
        }
        
        // Clamp the final size between the defined min and max values
        return Mathf.Clamp(requiredSize, minSize, maxSize);
    }

    // Calculates the target position for the camera
    private Vector3 CalculateTargetPosition()
    {
        // Find the midpoint between the two players
        Vector3 centerPoint = (player1.position + player2.position) / 2f;
        
        // Use the current camera size to calculate the clamping boundaries
        float camHalfHeight = mainCamera.orthographicSize;
        float camHalfWidth = camHalfHeight * mainCamera.aspect;
        
        // Clamp the center point to stay within the level bounds
        float clampedX = Mathf.Clamp(centerPoint.x, minBounds.position.x + camHalfWidth, maxBounds.position.x - camHalfWidth);
        float clampedY = Mathf.Clamp(centerPoint.y, minBounds.position.y + camHalfHeight, maxBounds.position.y - camHalfHeight);
        
        // Return the final clamped position with the Z offset
        return new Vector3(clampedX, clampedY, zOffset);
    }

    // A function to instantly set the camera's state at the beginning of the game
    private void SetInitialCameraState()
    {
        if (player1 == null || player2 == null || minBounds == null || maxBounds == null)
        {
            Debug.LogError("Camera targets or bounds not set!");
            return;
        }

        mainCamera.orthographicSize = CalculateTargetSize();
        transform.position = CalculateTargetPosition();
    }
}