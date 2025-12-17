using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private KeyCode openKey = KeyCode.E;
    [SerializeField] private Animator anim;
    [SerializeField] private Collider2D doorCollider;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI instructionText; // Just assign the text directly
    [SerializeField] private string openInstruction = "Press E to Open";
    [SerializeField] private string needKeyInstruction = "You need a key!";

    [Header("Audio")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip lockedSound;
    [SerializeField] private AudioClip checkpointSound;

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = false;

    private bool playerInRange = false;
    private bool isOpened = false;
    private Player player;

    private string[] roomMessages;

    private void Start()
    {
        roomMessages = new string[]
            {
                "A new trial begins.",
                "You step deeper into the dark.",
                "Another chamber awakens.",
                "Something stirs…",
                "The dungeon tightens its grip."
            };
        if (showDebugLogs) Debug.Log("[Door] Door initialized: " + gameObject.name);

        if (instructionText != null)
        {
            instructionText.gameObject.SetActive(false);
            if (showDebugLogs) Debug.Log("[Door] Instruction text found and hidden");
        }
        else
        {
            Debug.LogWarning("[Door] Instruction text is not assigned!");
        }

        // Get door collider if not assigned
        if (doorCollider == null)
        {
            doorCollider = GetComponent<Collider2D>();
            if (showDebugLogs) Debug.Log("[Door] Door collider auto-assigned");
        }

        if (doorCollider == null)
        {
            Debug.LogError("[Door] No collider found! Door won't work properly.");
        }

        if (anim == null)
        {
            Debug.LogWarning("[Door] Animator is not assigned!");
        }
    }

    private void Update()
    {

        instructionText.transform.position = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z);

        if (!playerInRange || isOpened) return;

        if (Input.GetKeyDown(openKey))
        {
            if (showDebugLogs) Debug.Log("[Door] Player pressed " + openKey);

            if (player != null && player.HasKey())
            {
                if (showDebugLogs) Debug.Log("[Door] Player has key! Opening door...");
                OpenDoor();
            }
            else
            {
                if (showDebugLogs)
                {
                    if (player == null)
                        Debug.LogWarning("[Door] Player reference is null!");
                    else
                        Debug.Log("[Door] Player doesn't have a key");
                }

                // Play locked sound
                if (lockedSound != null)
                    SoundManager.instance.PlaySound(lockedSound);
            }
        }
    }

    private void OpenDoor()
    {
        if (showDebugLogs) Debug.Log("[Door] Opening door!");

        isOpened = true;

        // Play animation
        if (anim != null)
        {
            anim.SetTrigger("open");
            if (showDebugLogs) Debug.Log("[Door] Animation triggered");
            if (player)
            {
                player.checkpoint = transform.position;
            }
        }
        else
        {
            Debug.LogWarning("[Door] No animator! Animation won't play.");
        }

        // Play sound
        if (openSound != null)
            SoundManager.instance.PlaySound(openSound);

        // Disable collider so player can pass
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
            if (showDebugLogs) Debug.Log("[Door] Collider disabled - player can pass");
        }

        // Hide UI
        if (instructionText != null)
            instructionText.gameObject.SetActive(false);

        // Use the key
        if (player != null)
        {
            player.UseKey();
            if (showDebugLogs) Debug.Log("[Door] Key consumed from player inventory");
            DialogueManager.Instance.ShowDialogue(roomMessages[Random.Range(0, roomMessages.Length)]);
            SoundManager.instance.PlaySound(checkpointSound);
        }
    }

    private void ShowInstruction(bool hasKey)
    {
        if (instructionText != null)
        {
            instructionText.gameObject.SetActive(true);
            instructionText.text = hasKey ? openInstruction : needKeyInstruction;

            // Optional: change text color based on state
            instructionText.color = hasKey ? Color.white : Color.red;

            if (showDebugLogs)
                Debug.Log("[Door] Showing instruction: " + instructionText.text + " (Has Key: " + hasKey + ")");
        }
        else
        {
            Debug.LogWarning("[Door] Cannot show instruction - text not assigned!");
        }
    }

    private void HideInstruction()
    {
        if (instructionText != null)
        {
            instructionText.gameObject.SetActive(false);
            if (showDebugLogs) Debug.Log("[Door] Instruction hidden");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (showDebugLogs) Debug.Log("[Door] Trigger entered by: " + other.name + " (Tag: " + other.tag + ")");

        if (other.CompareTag("Player") && !isOpened)
        {
            if (showDebugLogs) Debug.Log("[Door] Player entered trigger zone!");

            playerInRange = true;
            player = other.GetComponent<Player>();

            if (player == null)
            {
                Debug.LogError("[Door] Player object has no Player component!");
            }

            // Show appropriate instruction
            bool hasKey = player != null && player.HasKey();
            if (showDebugLogs) Debug.Log("[Door] Player has key: " + hasKey);
            ShowInstruction(hasKey);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (showDebugLogs) Debug.Log("[Door] Player left trigger zone");

            playerInRange = false;
            player = null;
            HideInstruction();
        }
    }
}