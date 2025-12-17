using System.Collections;
using Mono.Cecil.Cil;
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
    [SerializeField] private bool flyingEnemy = false;

    private float cooldownTimer = Mathf.Infinity;
    public bool isAttacking = false;

    public bool IsAttacking => isAttacking;
    public bool IsDead;

    private Player player;
    private Animator anim;

    public int FacingDirection { get; private set; } = 1;

    [Header("Back Detection")]
    [Header("Back Detection")]
    [SerializeField] private float backDetectionRange = 0.8f;
    [SerializeField] private float backDetectionWidth = 1.2f;



    public void SetFacingDirection(int direction)
    {
        FacingDirection = direction;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        isAttacking = false;
        IsDead = false;
    }

    private void Update()
    {
        if (IsDead) return;

        cooldownTimer += Time.deltaTime;

        // Player behind -> turn
        if (PlayerBehind() && !isAttacking)
        {
            TurnTowardsPlayer();
            return;
        }

        // Player in front -> attack
        if (PlayerInSight() && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
            isAttacking = true;
            anim.SetTrigger("attack");
        }

        if (!PlayerInSight() && flyingEnemy)
        {
            EnableMovement();
        }
    }
    private void TurnTowardsPlayer()
    {
        // Flip sprite
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // Sync patrol logic
        EnemyPatrol patrol = GetComponent<EnemyPatrol>();
        if (patrol != null)
            patrol.ForceTurn();

        // STOP movement briefly to avoid moonwalk / jitter
        isAttacking = true;
        Invoke(nameof(EnableMovement), 0.1f);

        cooldownTimer = attackCooldown * 0.5f;
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
            player = hit.transform.GetComponent<Player>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        // Front
        Gizmos.color = Color.red;
        Vector3 dir = transform.localScale.x > 0 ? Vector3.right : Vector3.left;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + dir * range * colliderDistance,
            new Vector3(boxCollider.bounds.size.x + range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );

        // Back
        Gizmos.color = Color.blue;
        Vector3 backDir = transform.localScale.x > 0 ? Vector3.left : Vector3.right;
        Vector2 backSize = new Vector2(
            boxCollider.bounds.size.x * backDetectionWidth,
            boxCollider.bounds.size.y
        );
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + backDir * backDetectionRange,
            backSize
        );

    }
    public bool PlayerBehind()
    {
        Vector3 backDirection = transform.localScale.x > 0 ? Vector3.left : Vector3.right;

        Vector2 boxSize = new Vector2(
            boxCollider.bounds.size.x * backDetectionWidth,
            boxCollider.bounds.size.y
        );

        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + backDirection * backDetectionRange,
            boxSize,
            0,
            Vector2.zero,   // IMPORTANT: no cast movement
            0,
            playerLayer
        );

        if (hit.collider != null)
            player = hit.transform.GetComponent<Player>();

        return hit.collider != null;
    }


    // Called by ANIMATION EVENT
    private void DamagePlayer()
    {
        if (player != null && PlayerInSight())
            player.TakeDamage(damage);
    }

    // Called by ANIMATION EVENT
    public void EnableMovement()
    {
        isAttacking = false;
    }
}
