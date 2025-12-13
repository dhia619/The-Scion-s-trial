using UnityEngine;

public class AxeSwing2D : MonoBehaviour
{
    [Header("Swing Settings")]
    public float swingAngle = 30f; // How far it swings in degrees
    public float swingSpeed = 2f; // How fast it swings
    public bool startSwinging = true;
    
    [Header("Debug")]
    public bool showDebug = true;
    
    private Vector3 initialRotation;
    private float timer;

    void Start()
    {
        // Store the starting rotation
        initialRotation = transform.eulerAngles;
        
        if (showDebug)
        {
        }
    }

    void Update()
    {
        if (!startSwinging) return;
        
        timer += Time.deltaTime;
        
        // Calculate swing angle using sine wave
        float currentAngle = Mathf.Sin(timer * swingSpeed) * swingAngle;
        
        // Apply rotation around Z-axis for 2D left-right swing
        transform.rotation = Quaternion.Euler(0f, 0f, initialRotation.z + currentAngle);
        
        // Debug info
        if (showDebug && Time.frameCount % 120 == 0) // Every 2 seconds
        {
        }
    } 
    public void OnCollisionEnter2D(Collision2D collider){
        if (collider.gameObject.CompareTag("Player")){
            collider.gameObject.GetComponent<Health>().TakeDamage(10);
        }
    }
}