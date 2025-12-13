using UnityEngine;

public class FallingChandelierAnim : MonoBehaviour
{
    [Header("Animation Settings")]
    public float detectionRange = 5f;
    public float fallDistance = 3f;
    public float fallDuration = 0.5f;
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.3f;
    
    private bool hasFallen = false;
    private bool isAnimating = false;
    private Vector3 originalPosition;
    private Transform player;
    private float animationTimer;

    void Start()
    {
        originalPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (hasFallen || isAnimating) return;
        
        // Check if player is in range
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            StartFallAnimation();
        }
    }
    
    void StartFallAnimation()
    {
        isAnimating = true;
        animationTimer = 0f;
    }

    void FixedUpdate()
    {
        if (!isAnimating) return;
        
        animationTimer += Time.deltaTime;
        
        if (animationTimer < shakeDuration)
        {
            // Shake phase
            float shakeX = Mathf.Sin(Time.time * 50f) * shakeIntensity;
            float shakeY = Mathf.Cos(Time.time * 40f) * shakeIntensity * 0.5f;
            transform.position = originalPosition + new Vector3(shakeX, shakeY, 0f);
        }
        else if (animationTimer < shakeDuration + fallDuration)
        {
            // Fall phase
            float fallProgress = (animationTimer - shakeDuration) / fallDuration;
            float newY = originalPosition.y - (fallProgress * fallDistance);
            transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
        }
        else
        {
            // Animation complete
            hasFallen = true;
            isAnimating = false;
            // Final position
            transform.position = new Vector3(originalPosition.x, originalPosition.y - fallDistance, originalPosition.z);
        }
    }
    
    // Visualize detection range in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}