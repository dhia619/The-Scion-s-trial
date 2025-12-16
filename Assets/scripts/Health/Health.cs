using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private float currentHealth;
    private Animator anim;
    private bool dead;
    private FloatingHealthBar healthBar;
    string[] messages;

    void Start()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        healthBar = GetComponent<FloatingHealthBar>();
        messages = new string[]{
                "Try again.",
                "You’ve got this.",
                "Rise again.",
                "Don’t give up.",
                "One more time."
            };
    }

    public void TakeDamage(float damage)
    {
        if (dead) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        healthBar.UpdateHealthBar(currentHealth, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
        }
        else
        {
            dead = true;
            anim.SetTrigger("die");
            if (CompareTag("Player"))
                StartCoroutine(PlayerDieAndRespawn());       
        }
    }

    private IEnumerator PlayerDieAndRespawn()
    {
        SoundManager.instance.PlaySound(GetComponent<Player>().deathSound);
        yield return new WaitForSeconds(2f);

        Player player = GetComponent<Player>();
        transform.position = player.checkpoint;

        currentHealth = startingHealth;
        healthBar.UpdateHealthBar(currentHealth, startingHealth);

        dead = false;
        anim.SetTrigger("idle");

        DialogueManager.Instance.ShowDialogue(messages[Random.Range(0, messages.Length)]);
    }



    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetCurrentHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public float GetMaxHealth()
    {
        return startingHealth;
    }

    public bool GetDead()
    {
        return dead;
    }
}
