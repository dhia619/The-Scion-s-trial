using NUnit.Framework.Constraints;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private float currentHealth;
    private Animator anim;
    private bool dead;
    private BoxCollider2D boxCollider;

    void Start()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        
        if(currentHealth > 0)
        {
            anim.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                dead = true;
                boxCollider.enabled = false;
            }
        }
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
}
