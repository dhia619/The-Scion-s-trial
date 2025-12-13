using UnityEngine;

public class GoblinController : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float range = 1f;
    [SerializeField] private float colliderDistance = 0.5f;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float throwForceX = 5f;
    [SerializeField] private float throwForceY = 3f;

    void Start()
    {
        
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
        return hit.collider != null;
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight() && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
            ThrowBomb();
        }
    }

    private void ThrowBomb()
    {
        // Create the bomb at the firePoint position
        GameObject bombObj = Instantiate(bombPrefab, firePoint.position, Quaternion.identity);

        // Get bomb script
        Bomb bomb = bombObj.GetComponent<Bomb>();

        // Direction depends on goblin facing
        float direction = transform.localScale.x > 0 ? 1f : -1f;

        // Apply force
        Vector2 force = new Vector2(throwForceX * direction, throwForceY);
        bomb.ThrowBomb(force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 direction = transform.localScale.x > 0 ? Vector3.right : Vector3.left;

        Gizmos.DrawWireCube(
            boxCollider.bounds.center + direction * range * colliderDistance,
            new Vector3(boxCollider.bounds.size.x + range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );
    }
}
