using UnityEngine;
using System.Collections;
using TMPro;

public class DoorTeleporter : MonoBehaviour
{
    public GameObject otherDoor;  
    public KeyCode teleportKey;
    public float teleportDelay = 2f;
    public float cooldownTime = 0.5f;
    
    private bool playerInRange = false;
    private bool canTeleport = true;
    private bool isTeleporting = false;
    private float teleportTimer = 0f;

    [SerializeField] private AudioClip teleportSound;

    [Header("Instruction UI")]
    [SerializeField] private TextMeshProUGUI instructionText;

    private void Start()
    {
        teleportKey = BindingManager.Instance.GetControl("Open /Teleport");
        if (instructionText != null)
            instructionText.gameObject.SetActive(false);
    }


    void Update()
    {
        instructionText.transform.position = new Vector3(transform.position.x + 6, transform.position.y + 4, transform.position.z); 
        // Start teleport when key pressed
        if (playerInRange && (Input.GetKeyDown(KeyCode.E) || Input.GetButton("Submit")) && canTeleport && otherDoor != null)
        {
            StartTeleport();
        }
        
        // Update teleport timer if teleporting
        if (isTeleporting)
        {
            teleportTimer += Time.deltaTime;
            
            // Optional: Update visual progress
            // Example: Change particle color based on progress
            
            if (teleportTimer >= teleportDelay)
            {
                ExecuteTeleport();
            }
        }
    }

    void StartTeleport()
    {
        isTeleporting = true;
        teleportTimer = 0f;
        canTeleport = false;

        if (instructionText != null)
            instructionText.gameObject.SetActive(false);

        Debug.Log($"Teleport charging... {teleportDelay} seconds");
    }

    void ExecuteTeleport()
    {
        isTeleporting = false;
        
        // Find and teleport player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = otherDoor.transform.position;
            Debug.Log($"Teleported to {otherDoor.name}!");
        }
        
        // Start cooldown
        StartCoroutine(CooldownRoutine());
    }
    
    void CancelTeleport()
    {
        isTeleporting = false;
        teleportTimer = 0f;        
            
    }
    
    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldownTime);
        canTeleport = true;
        SoundManager.instance.PlaySound(teleportSound);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (instructionText != null)
                instructionText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (instructionText != null)
                instructionText.gameObject.SetActive(false);

            // Cancel teleport if player leaves
            if (isTeleporting)
            {
                CancelTeleport();
                canTeleport = true;
            }
        }
    }

}