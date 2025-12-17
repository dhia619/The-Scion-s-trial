using UnityEngine;

public class HeartController : MonoBehaviour
{
    [SerializeField] private int healthToAdd;
    private FloatingHealthBar healthBar;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();

            if (playerHealth != null)
            {
                healthBar = collision.GetComponent<FloatingHealthBar>();
                playerHealth.SetCurrentHealth(playerHealth.GetCurrentHealth() + healthToAdd);
                healthBar.UpdateHealthBar(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
                gameObject.SetActive(false);
            }
        }
    }
}
