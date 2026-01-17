using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown;
    public float attackCooldownTimer = 0f;
    public bool isAttacking = false;
    private int attackAnimationIndex = 1;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float damage;
    [SerializeField] private float damageRadius = 0.5f;
    [SerializeField] private LayerMask enemiesLayerMask;
    [SerializeField] private AudioClip swordSound;
    [SerializeField] private AudioClip hurtSound;

    [Header("References")]
    public Animator animator;

    KeyCode attackKey;

    void Start()
    {
        animator = GetComponent<Animator>();
        attackKey = BindingManager.Instance.GetControl("Attack");
    }

    void Update()
    {
        if (GetComponent<Health>().GetDead()) return;
        attackCooldownTimer += Time.deltaTime;
        isAttacking = false;
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(attackKey) || Input.GetKeyDown(KeyCode.F) || Input.GetButton("Fire1"))
        {
            PlayAttackAnimation();
        }
    }

    private void PlayAttackAnimation()
    {
        if (!isAttacking && attackCooldownTimer >= attackCooldown)
        {
            attackAnimationIndex++;
            if (attackAnimationIndex > 3)
            {
                attackAnimationIndex = 1;
            }
            animator.SetTrigger("attack_" + attackAnimationIndex);

            isAttacking = true;
            attackCooldownTimer = 0f;
            SoundManager.instance.PlaySound(swordSound);
        }
    }

    public void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(firePoint.position, damageRadius, enemiesLayerMask);

        foreach (Collider2D enemy in enemies)
        {
            Health enemyHealth = enemy.gameObject.GetComponent<Health>();
            enemyHealth.TakeDamage(damage);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(firePoint.position, damageRadius);
    }

    public void PlayHurtSound()
    {
        SoundManager.instance.PlaySound(hurtSound);
    }

}
