using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown = 1f;
    public float attackCooldownTimer = 0f;
    public bool isAttacking = false;
    private int attackAnimationIndex = 1;

    [Header("References")]
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        attackCooldownTimer += Time.deltaTime;
        isAttacking = false;
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
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
        }
    }
}
