using UnityEngine;

public class FallingChandelierAnim : MonoBehaviour
{
    [Header("Animation Settings")]
    public float detectionRange = 10f;
    public float fallDistance = 7f;
    public float fallDuration = 0.5f;
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.3f;
    [SerializeField] private int damage = 20;

    private bool hasDealtDamage = false;
    private bool hasFallen = false;
    private bool isAnimating = false;
    private Vector3 originalPosition;
    private Transform player;
    private float animationTimer;

    [SerializeField] private AudioClip crashSound;

    void Start()
    {
        originalPosition = transform.position;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure Player has 'Player' tag.");
        }
    }

    void Update()
    {
        // Animation logic
        if (isAnimating)
        {
            UpdateAnimation();
            return;
        }

        // Don't check if already fallen or no player reference
        if (hasFallen || player == null) return;

        // Check distance on both X and Y axis (horizontal trigger)
        float horizontalDistance = Mathf.Abs(transform.position.x - player.position.x);

        // Trigger when player is directly below (within detection range horizontally)
        if (horizontalDistance <= detectionRange)
        {
            StartFallAnimation();
        }
    }

    void StartFallAnimation()
    {
        isAnimating = true;
        animationTimer = 0f;
    }

    void UpdateAnimation()
    {
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
            // Fall phase with smooth interpolation
            float fallProgress = (animationTimer - shakeDuration) / fallDuration;
            // Use ease-in for more realistic fall
            float easedProgress = fallProgress * fallProgress;
            float newY = originalPosition.y - (easedProgress * fallDistance);
            transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
        }
        else
        {
            // Animation complete
            hasFallen = true;
            isAnimating = false;
            SoundManager.instance.PlaySound(crashSound);
            // Final position
            transform.position = new Vector3(originalPosition.x, originalPosition.y - fallDistance, originalPosition.z);
        }
    }

    // Visualize detection range in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 topLeft = transform.position + new Vector3(-detectionRange, 0, 0);
        Vector3 topRight = transform.position + new Vector3(detectionRange, 0, 0);
        Vector3 bottomLeft = topLeft + new Vector3(0, -fallDistance, 0);
        Vector3 bottomRight = topRight + new Vector3(0, -fallDistance, 0);

        // Draw detection zone
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomLeft, bottomRight);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasDealtDamage) return;

        if (collision.CompareTag("Player"))
        {
            Player playerScript = collision.GetComponent<Player>();
            if (playerScript != null && !hasFallen)
            {
                playerScript.TakeDamage(damage);
                hasDealtDamage = true;
            }
        }
    }
}