using UnityEngine;

public class ScaryFace : MonoBehaviour
{
    [Header("Float")]
    public float floatSpeed = 2f;
    public float floatAmount = 9f;

    [Header("Blink")]
    public float blinkInterval = 4f;

    private Vector3 startPos;
    private Animator animator;
    private float blinkTimer;

    void Start()
    {
        startPos = transform.localPosition;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Floating vibe
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.localPosition = startPos + Vector3.up * yOffset;

        // Blink timer
        blinkTimer += Time.deltaTime;
        if (blinkTimer >= blinkInterval)
        {
            animator.SetTrigger("blink");
            blinkTimer = 0f;
        }
    }
}
