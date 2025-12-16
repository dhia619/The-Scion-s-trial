using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    [Header("Loot")]
    [SerializeField] private GameObject[] lootPrefabs;
    [SerializeField] private float lootSpawnHeight = 1f;
    [SerializeField] private float throwForceX = 2f;
    [SerializeField] private float throwForceY = 5f;
    [SerializeField] private float lootScale = 1.5f; // Make loot bigger

    [Header("Instruction UI")]
    [SerializeField] private TextMeshProUGUI instructionText;

    private bool playerInRange = false;
    private float holdTimer = 0f;
    private bool opened = false;

    private void Start()
    {
        if (progressBar != null)
            progressBar.gameObject.SetActive(false);

        if (instructionText != null)
            instructionText.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (!playerInRange || opened) return;

        if (Input.GetKey(openKey))
        {
            holdTimer += Time.deltaTime;
            float fill = holdTimer / holdTime;

            progressBar.gameObject.SetActive(true);
            progressBar.fillAmount = fill;
            instructionText.gameObject.SetActive(false);

            if (holdTimer >= holdTime)
                OpenChest();
        }
        else
        {
            holdTimer = 0;

            if (progressBar != null)
            {
                progressBar.fillAmount = 0;
                progressBar.gameObject.SetActive(false);
                instructionText.gameObject.SetActive(true);
            }
        }

    }


    private void OpenChest()
    {
        if (opened) return;

        opened = true;

        instructionText.gameObject.SetActive(false);

        if (progressBar != null)
            progressBar.gameObject.SetActive(false);

        anim?.SetTrigger("open");
        SoundManager.instance.PlaySound(openSound);
    }

    private void SpawnLoot()
    {
        if (lootPrefabs.Length == 0)
        {
            Debug.LogWarning("Chest has no loot assigned!");
            return;
        }

        int index = Random.Range(0, lootPrefabs.Length);
        GameObject lootPrefab = lootPrefabs[index];

        Vector3 spawnPos = transform.position + Vector3.up * lootSpawnHeight;
        GameObject lootInstance = Instantiate(lootPrefab, spawnPos, Quaternion.identity);

        // SCALE THE LOOT
        lootInstance.transform.localScale *= lootScale;

        // Apply force
        Rigidbody2D rb = lootInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float randomX = Random.Range(-throwForceX, throwForceX);
            rb.AddForce(new Vector2(randomX, throwForceY), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !opened)
        {
            playerInRange = true;
            instructionText.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            holdTimer = 0;

            instructionText.gameObject.SetActive(false);
            progressBar.gameObject.SetActive(false);
        }
    }

}