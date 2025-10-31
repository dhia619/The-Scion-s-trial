using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool isAttacking = false;
    public float attackCooldown = 1;
    public float attackCooldownTimer = Mathf.Infinity;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (!isAttacking && attackCooldownTimer > attackCooldown)
        {
            print("attack");
            attackCooldownTimer = 0;
            isAttacking = true;
        }
        else
        {
            attackCooldownTimer += Time.deltaTime;
            isAttacking = false;
        }
    }
}
