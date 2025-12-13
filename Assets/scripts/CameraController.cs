using UnityEngine;
using UnityEngine.Rendering.Universal; // For Light 2D

public class CameraController : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float maxSpeed = 10f;
    
    [Header("Camera Distance")]
    [SerializeField] private float yOffset = 3f;
    [SerializeField] private float xOffset = 0f;
    
    [Header("Dead Zone")]
    [SerializeField] private float deadZoneRadius = 2f;
    [SerializeField] private bool useDeadZone = true;
    
    [Header("Boundaries")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;
    
    [Header("Light 2D Settings")]
    [SerializeField] private Light2D playerLight;
    [SerializeField] private float lightSmoothSpeed = 8f;
    
    public Transform target;
    private Vector3 velocity = Vector3.zero;
    private float currentPosX;

    private void LateUpdate()
    {
        if (target == null) return;

        // Calculate target position with offsets
        float targetX = target.position.x + xOffset;
        float targetY = target.position.y + yOffset;
        
        Vector3 targetPosition = new Vector3(targetX, targetY, -10f);
        
        // Apply dead zone
        if (useDeadZone)
        {
            float distanceToPlayer = Vector2.Distance(
                new Vector2(transform.position.x, transform.position.y),
                new Vector2(targetPosition.x, targetPosition.y)
            );

            // Only move camera if player is outside dead zone
            if (distanceToPlayer <= deadZoneRadius)
            {
                return; // Don't move camera
            }
        }
        
        // Apply boundaries
        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
        }
        
        // Smooth camera movement
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, maxSpeed);
        
        // Update light position to center on player
        UpdateLightPosition();
    }
    
    private void UpdateLightPosition()
    {
        if (playerLight != null && target != null)
        {
            // Light always centers on player (not camera)
            Vector3 lightTargetPos = target.position;
            lightTargetPos.z = 0; // Keep light at Z=0 for 2D
            
            playerLight.transform.position = Vector3.Lerp(
                playerLight.transform.position, 
                lightTargetPos, 
                lightSmoothSpeed * Time.deltaTime
            );
        }
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
    
    // Light control methods
    public void SetLightIntensity(float intensity)
    {
        if (playerLight != null)
            playerLight.intensity = intensity;
    }
    
    public void SetLightColor(Color color)
    {
        if (playerLight != null)
            playerLight.color = color;
    }
    
    public void EnableLight(bool enable)
    {
        if (playerLight != null)
            playerLight.enabled = enable;
    }
    
    // Public methods to control camera behavior
    public void SetYOffset(float newYOffset)
    {
        yOffset = newYOffset;
    }
    
    public void SetDeadZoneRadius(float newRadius)
    {
        deadZoneRadius = newRadius;
    }
    
    public void SetBounds(Vector2 newMinBounds, Vector2 newMaxBounds)
    {
        minBounds = newMinBounds;
        maxBounds = newMaxBounds;
        useBounds = true;
    }
    
    public void DisableBounds()
    {
        useBounds = false;
    }
    
    public void EnableDeadZone(bool enable)
    {
        useDeadZone = enable;
    }
    
    // Visualize dead zone and bounds in Scene view
    private void OnDrawGizmosSelected()
    {
        // Draw dead zone
        if (useDeadZone)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, deadZoneRadius);
        }
        
        // Draw bounds
        if (useBounds)
        {
            Gizmos.color = Color.red;
            Vector3 center = new Vector3((minBounds.x + maxBounds.x) * 0.5f, (minBounds.y + maxBounds.y) * 0.5f, -10f);
            Vector3 size = new Vector3(maxBounds.x - minBounds.x, maxBounds.y - minBounds.y, 0.1f);
            Gizmos.DrawWireCube(center, size);
        }
        
        // Draw light center on player
        if (playerLight != null && target != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(target.position, 0.5f);
        }
    }
}