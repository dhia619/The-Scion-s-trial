using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private KeyCode openKey;
    [SerializeField] private Animator anim;
    [SerializeField] private bool isFinalDoor = false;
    [SerializeField] private Collider2D doorCollider;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI instructionText; 
    [SerializeField] private string openInstruction = "Press E to Open";
    [SerializeField] private string needKeyInstruction = "You need a key!";

    [Header("Audio")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip lockedSound;
    [SerializeField] private AudioClip checkpointSound;

    private bool playerInRange = false;
    private bool isOpened = false;
    private Player player;
    private string[] roomMessages;
    

    private void Start()
    {
        openKey = BindingManager.Instance.GetControl("Open /Teleport");
        roomMessages = new string[]
        {
            "A new trial begins.",
            "You step deeper into the dark.",
            "Another chamber awakens.",
            "Courage guides your path.",
            "The dungeon tightens its grip."
        };
        
        instructionText.gameObject.SetActive(false);

        // Get door collider if not assigned
        if (doorCollider == null)
        {
            doorCollider = GetComponent<Collider2D>();
        }

        if (doorCollider == null)
        {
            Debug.LogError("[Door] No collider found! Door won't work properly.");
        }

        if (anim == null && !isFinalDoor) // Only warn for non-final doors
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
            if (player != null && player.HasKey())
            {
                OpenDoor();
            }
            else
            {
                if (lockedSound != null)
                    SoundManager.instance.PlaySound(lockedSound);
            }
        }
    }

    private void OpenDoor()
    {
        isOpened = true;

        if (isFinalDoor)
        {
            if (player != null)
            {
                player.UseKey(); 
            }
            LevelManager.Instance.LoadScene("OutroCutscene", "CrossFade");
            return; 
        }

        // Normal door behavior
        if (anim != null)
        {
            anim.SetTrigger("open");
            if (player)
            {
                player.checkpoint = transform.position;
                CheckPointManager.Instance?.SaveCheckpoint(transform.position);
            }
        }

        if (openSound != null)
            SoundManager.instance.PlaySound(openSound);

        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        if (instructionText != null)
            instructionText.gameObject.SetActive(false);

        if (player != null)
        {
            player.UseKey();
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
            instructionText.color = hasKey ? Color.white : Color.red;
        }
    }

    private void HideInstruction()
    {
        if (instructionText != null)
        {
            instructionText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            playerInRange = true;
            player = other.GetComponent<Player>();
            bool hasKey = player != null && player.HasKey();
            ShowInstruction(hasKey);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            HideInstruction();
        }
    }
}