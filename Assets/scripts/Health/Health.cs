using System.Collections;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private float currentHealth;
    private Animator anim;
    private bool dead;
    private FloatingHealthBar healthBar;
    string[] messages;

    [Header("Death Transition")]
    [SerializeField] private GameObject DeathScene;
    [SerializeField] private CrossFade deathTransition; 
    // [SerializeField] private float deathTransitionDuration = 2f;
    [SerializeField] private TextMeshProUGUI deathMessageText;  

    void Start()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        healthBar = GetComponent<FloatingHealthBar>();
        messages = new string[]{
                "Try again.",
                "You have got this.",
                "Rise again.",
                "Do not give up.",
                "One more time."
            };
        
        if (deathTransition != null && deathTransition.crossFade != null)
        {
            deathMessageText.gameObject.SetActive(false);
            deathTransition.crossFade.alpha = 0f;
            deathTransition.crossFade.gameObject.SetActive(false);
        }
        
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
            DeathScene.SetActive(true);
            if (CompareTag("Player"))
                StartCoroutine(PlayerDieAndRespawn());       
        }
    }

    private IEnumerator PlayerDieAndRespawn()
    {
        
        
        SoundManager.instance.PlaySound(GetComponent<Player>().deathSound);
        if (deathTransition != null)
        {
            deathMessageText.gameObject.SetActive(true);
            yield return StartCoroutine(deathTransition.AnimateTransitionIn());
        }
        yield return new WaitForSeconds(1f);
        
        Player player = GetComponent<Player>();
        transform.position = player.checkpoint;

        currentHealth = startingHealth;
        healthBar.UpdateHealthBar(currentHealth, startingHealth);

        dead = false;
        anim.SetTrigger("idle");

        if (deathTransition != null)
        {
            deathMessageText.gameObject.SetActive(false);
            yield return StartCoroutine(deathTransition.AnimateTransitionOut());
        }


        DialogueManager.Instance.ShowDialogue(messages[Random.Range(0, messages.Length)]);
        DeathScene.SetActive(false);
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
