using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float range = 1f;
    [SerializeField] private float colliderDistance = 0.5f;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer = Mathf.Infinity;
    private bool isAttacking = false;

    public bool IsAttacking => isAttacking;
    public bool IsDead => GetComponent<Health>().GetDead();

    private Health playerHealth;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsDead) return;

        cooldownTimer += Time.deltaTime;

        if (PlayerInSight() && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
            isAttacking = true;
            anim.SetTrigger("attack");
        }
    }

    public void Desactivate()
    {
        gameObject.SetActive(false);
    }

    public bool PlayerInSight()
    {
        Vector3 direction = transform.localScale.x > 0 ? Vector3.right : Vector3.left;

        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + direction * range * colliderDistance,
            new Vector3(boxCollider.bounds.size.x + range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0,
            direction,
            0,
            playerLayer
        );

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 direction = transform.localScale.x > 0 ? Vector3.right : Vector3.left;

        Gizmos.DrawWireCube(
            boxCollider.bounds.center + direction * colliderDistance,
            new Vector3(boxCollider.bounds.size.x + range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );
    }

    // Called by ANIMATION EVENT
    private void DamagePlayer()
    {
        if (playerHealth != null && PlayerInSight())
            playerHealth.TakeDamage(damage);
    }

    // Called by ANIMATION EVENT
    public void EnableMovement()
    {
        isAttacking = false;
    }
}
