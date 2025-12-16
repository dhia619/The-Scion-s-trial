using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    public float floatSpeed = 4f;
    public float floatAmplitude = 0.1f;

    [Header("Sound")]
    [SerializeField] private AudioClip collectSound;

    [Header("Player Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float distanceToShow = 2f;
    [SerializeField] private float offset = 1f;

    private Vector3 startPos;
    private bool hintShown = false;

    private string[] messages;

    void Start()
    {
        startPos = transform.position;

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        messages = new string[]
        {
            "Something glints nearby…",
            "You sense something useful close.",
            "A faint metallic sound echoes.",
            "Something important is close.",
            "You feel a pull toward something."
        };
    }

    void Update()
    {
        transform.position = startPos +
            new Vector3(0, Mathf.Sin(Time.time * floatSpeed) * floatAmplitude, 0);

        if (player == null) return;

        float distanceToPlayer =
            Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < distanceToShow + offset && !hintShown)
        {
            DialogueManager.Instance.ShowDialogue(
                messages[Random.Range(0, messages.Length)]
            );
            hintShown = true;
        }

        if (distanceToPlayer >= distanceToShow + offset)
        {
            hintShown = false;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = distanceToPlayer < distanceToShow;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound(collectSound);
            gameObject.SetActive(false);
        }
    }
}
