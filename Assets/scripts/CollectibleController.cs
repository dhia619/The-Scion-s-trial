using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private float HealthToAdd;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision detected");   
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth.SetCurrentHealth(playerHealth.GetCurrentHealth() + HealthToAdd);
            gameObject.SetActive(false);
            Debug.Log("health added");
        }
    }
}
