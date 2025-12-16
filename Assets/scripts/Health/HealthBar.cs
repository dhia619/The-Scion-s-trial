using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public GameObject player;
    private float playerHealth, playerMaxHealth;
    void Start()
    {
        playerMaxHealth = player.GetComponent<Health>().GetMaxHealth();
    }

    void Update()
    {
        playerHealth = player.GetComponent<Health>().GetCurrentHealth();
        healthBar.fillAmount = Mathf.Clamp(playerHealth / playerMaxHealth, 0, 100);
    }
}
