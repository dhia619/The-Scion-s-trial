using UnityEngine;

public class HeartController : MonoBehaviour
{
    [SerializeField] private int healthToAdd;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.SetCurrentHealth(playerHealth.GetCurrentHealth() + healthToAdd);
                gameObject.SetActive(false);
            }
        }
    }
}
