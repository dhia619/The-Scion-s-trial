using UnityEngine;

public class AlternatingRocks : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 2f;
    public float moveSpeed = 1f;
    public float phaseOffset = 0f; // Stagger the movement (0-1)
    
    private Vector3 startPosition;
    private float timer;

    void Start()
    {
        startPosition = transform.position;
        timer = phaseOffset * Mathf.PI * 2f; // Offset based on phase
    }

    void Update()
    {
        timer += Time.deltaTime * moveSpeed;
        
        // Use sine wave for smooth up/down movement
        float newY = startPosition.y + Mathf.Sin(timer) * moveDistance;
        
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}