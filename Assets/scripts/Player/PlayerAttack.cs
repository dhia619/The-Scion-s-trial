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
        for (int i=0; i<fireballs.Length; i++)
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
        
    }
}
