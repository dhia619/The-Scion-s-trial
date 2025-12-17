using UnityEngine;

public class ChandelierAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public float detectionWidth = 1.5f;
    public float shakeDuration = 1f;
    public float shakeIntensity = 0.1f;
    public float fallDistance = 3f;
    public float fallDuration = 0.5f;
    
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
        if (isAnimating) return;
        
        // Check if player is directly under chandelier
        if (IsPlayerUnder())
        {
            StartAnimation();
        }
    }
    
    void FixedUpdate()
    {
        if (!isAnimating) return;
        
        animationTimer += Time.deltaTime;
        
        if (animationTimer < shakeDuration)
        {
            // Shake phase
            float shakeX = Mathf.Sin(Time.time * 50f) * shakeIntensity;
            float shakeY = Mathf.Cos(Time.time * 60f) * shakeIntensity * 0.5f;
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
            isAnimating = false;
        }
    }
    
    bool IsPlayerUnder()
    {
        float xDistance = Mathf.Abs(transform.position.x - player.position.x);
        bool isWithinWidth = xDistance <= detectionWidth / 2f;
        bool isBelow = player.position.y < transform.position.y;
        
        return isWithinWidth && isBelow;
    }
    
    void StartAnimation()
    {
        isAnimating = true;
        animationTimer = 0f;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 detectionArea = new Vector3(detectionWidth, 0.2f, 0);
        Gizmos.DrawWireCube(transform.position, detectionArea);
    }
}