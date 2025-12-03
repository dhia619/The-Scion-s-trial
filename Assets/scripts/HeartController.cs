using UnityEngine;

public class HeartController : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private float HealthToAdd;
    private Transform transform;
    [SerializeField] private AudioClip collectSound;

    public void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound(collectSound);
            playerHealth.SetCurrentHealth(playerHealth.GetCurrentHealth() + HealthToAdd);
            gameObject.SetActive(false);
        }
    }

    public void Start()
    {
        transform = GetComponent<Transform>();
    }

    public void Update()
    {
        transform.Rotate(0, 0.1f, 0);
    }
}
