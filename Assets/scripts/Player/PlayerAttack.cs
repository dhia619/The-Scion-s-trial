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
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private float damage;

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

        if (Input.GetKey(KeyCode.F))
        {
            ShootFireball();
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

    private void ShootFireball()
    {
        if (!isAttacking && attackCooldownTimer >= attackCooldown && GetComponent<FireSword>().GetFireSword())
        {
            isAttacking = true;
            attackCooldownTimer = 0f;
            fireballs[FindFireball()].transform.position = firePoint.position;
            fireballs[FindFireball()].GetComponent<Projectile>().Launch(Mathf.Sign(transform.localScale.x));
        }
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }

    public void OnAttackHit()
    {
        float attackRange = 1.2f;
        Vector2 direction = new Vector2(Mathf.Sign(transform.localScale.x), 0);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            hit.collider.GetComponent<Health>().TakeDamage(damage);
        }
    }

}
