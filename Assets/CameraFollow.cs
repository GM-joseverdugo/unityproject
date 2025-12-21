using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

    [Header("Camera Bounds (Optional)")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        // If no target assigned, try to find player
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                Debug.Log("Camera found player target automatically");
            }
            else
            {
                Debug.LogWarning("No target assigned and no GameObject with 'Player' tag found!");
            }
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate desired position
        Vector3 desiredPosition = target.position + offset;

        // Apply bounds if enabled
        if (useBounds)
        {
            // Calculate camera bounds based on orthographic size
            float camHeight = cam.orthographicSize;
            float camWidth = camHeight * cam.aspect;

            // Clamp camera position within bounds
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x + camWidth, maxBounds.x - camWidth);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y + camHeight, maxBounds.y - camHeight);
        }

        // Smoothly move camera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    // Optional: Set new target at runtime
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Optional: Instant snap to target (useful for scene transitions)
    public void SnapToTarget()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;

        if (useBounds)
        {
            float camHeight = cam.orthographicSize;
            float camWidth = camHeight * cam.aspect;
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x + camWidth, maxBounds.x - camWidth);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y + camHeight, maxBounds.y - camHeight);
        }

        transform.position = targetPosition;
    }
}
