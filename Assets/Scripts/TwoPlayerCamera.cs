using UnityEngine;
using System.Collections.Generic; // We need this to use List<T>

public class TwoPlayerCamera : MonoBehaviour
{
    // --- Settings in Inspector (No changes here) ---
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
        mainCamera = GetComponent<Camera>();
        // We need to handle the initial state carefully now
        if (player1 != null && player2 != null)
        {
            SetInitialCameraState();
        }
    }

    void LateUpdate()
    {
        // --- [CORE LOGIC CHANGE] ---
        // Create a list to hold only the active players
        List<Transform> activePlayers = new List<Transform>();
        if (player1 != null && player1.gameObject.activeInHierarchy)
        {
            activePlayers.Add(player1);
        }
        if (player2 != null && player2.gameObject.activeInHierarchy)
        {
            activePlayers.Add(player2);
        }

        // If there are no active players, do nothing.
        if (activePlayers.Count == 0)
        {
            return;
        }

        // Calculate and smoothly set the camera's size and position based on the active players
        float targetSize = CalculateTargetSize(activePlayers);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, smoothSpeed);
        
        Vector3 targetPosition = CalculateTargetPosition(activePlayers);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
    
    // --- REFACTORED FUNCTIONS ---

    // Calculates the target orthographic size based on a list of active targets
    private float CalculateTargetSize(List<Transform> targets)
    {
        // If only one player (or none) is active, just use the minimum zoom size.
        if (targets.Count <= 1)
        {
            return minSize;
        }
        
        // If two players are active, perform the original distance calculation
        float horizontalDistance = Mathf.Abs(targets[0].position.x - targets[1].position.x);
        float verticalDistance = Mathf.Abs(targets[0].position.y - targets[1].position.y);
        
        float requiredSizeByWidth = horizontalDistance * Screen.height / Screen.width * 0.5f;
        float requiredSizeByHeight = verticalDistance * 0.5f;
        
        float requiredSize = Mathf.Max(requiredSizeByWidth, requiredSizeByHeight);

        float buffer = 1.0f - screenEdgeBuffer;
        if (buffer > 0.01f)
        {
            requiredSize /= buffer;
        }
        
        return Mathf.Clamp(requiredSize, minSize, maxSize);
    }

    // Calculates the target position based on a list of active targets
    private Vector3 CalculateTargetPosition(List<Transform> targets)
    {
        Vector3 centerPoint;
        
        // If only one player is active, the center point is that player's position.
        if (targets.Count == 1)
        {
            centerPoint = targets[0].position;
        }
        // If two (or more) players are active, find their average center point.
        else
        {
            // For this project, we know it's only two, so we can be specific.
            centerPoint = (targets[0].position + targets[1].position) / 2f;
        }

        // The rest of the clamping logic remains the same
        float camHalfHeight = mainCamera.orthographicSize;
        float camHalfWidth = camHalfHeight * mainCamera.aspect;
        
        float clampedX = Mathf.Clamp(centerPoint.x, minBounds.position.x + camHalfWidth, maxBounds.position.x - camHalfWidth);
        float clampedY = Mathf.Clamp(centerPoint.y, minBounds.position.y + camHalfHeight, maxBounds.position.y - camHalfHeight);
        
        return new Vector3(clampedX, clampedY, zOffset);
    }
    
    // SetInitialCameraState needs to be updated to use a list as well
    private void SetInitialCameraState()
    {
        List<Transform> initialPlayers = new List<Transform> { player1, player2 };
        mainCamera.orthographicSize = CalculateTargetSize(initialPlayers);
        transform.position = CalculateTargetPosition(initialPlayers);
    }
}