using UnityEngine;
using UnityEngine.UI;

public class ChestController : MonoBehaviour
{
    [Header("Chest Settings")]
    [SerializeField] private float holdTime = 2f;
    [SerializeField] private KeyCode openKey = KeyCode.E;
    [SerializeField] private Animator anim;

    [Header("UI")]
    [SerializeField] private Image progressBar;
    [SerializeField] private Canvas progressCanvas;
    [SerializeField] private AudioClip openSound;

    private bool playerInRange = false;
    private float holdTimer = 0f;
    private bool opened = false;

    private void Start()
    {
        if (progressCanvas != null)
            progressCanvas.enabled = false;
    }

    private void Update()
    {
        if (!playerInRange || opened) return;

        // Start holding
        if (Input.GetKey(openKey))
        {
            holdTimer += Time.deltaTime;
            float fill = holdTimer / holdTime;

            if (progressBar != null)
                progressBar.fillAmount = fill;

            if (progressCanvas != null)
                progressCanvas.enabled = true;

            // Fully held -> open chest
            if (holdTimer >= holdTime)
            {
                OpenChest();
            }
        }
        else
        {
            // Released early -> cancel progress
            if (holdTimer > 0)
            {
                holdTimer = 0;

                if (progressBar != null)
                    progressBar.fillAmount = 0;
            }

            if (progressCanvas != null)
                progressCanvas.enabled = false;
        }
    }

    private void OpenChest()
    {
        opened = true;

        if (anim != null)
            anim.SetTrigger("open");

        if (progressCanvas != null)
            progressCanvas.enabled = false;

        SoundManager.instance.PlaySound(openSound);

        // TODO: Spawn loot here
        Debug.Log("Chest opened! Loot rewarded.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            holdTimer = 0;

            if (progressBar != null)
                progressBar.fillAmount = 0;

            if (progressCanvas != null)
                progressCanvas.enabled = false;
        }
    }
}
