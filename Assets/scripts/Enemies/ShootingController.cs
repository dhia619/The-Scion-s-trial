using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private Transform firePoint;            // position where projectile spawns
    [SerializeField] private GameObject[] projectiles;       // list of projectile prefabs or pooled objects
    [SerializeField] private float shootCooldown = 5f;       // seconds between shots

    private float shootElapsedTime = 0f;
    private EnemyController enemyController;
    private Animator anim;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        shootElapsedTime += Time.deltaTime;

        // Enemy only shoots if player is detected and cooldown reached
        if (enemyController.PlayerInSight() && shootElapsedTime >= shootCooldown)
        {
            shootElapsedTime = 0;
            if (anim != null)
                anim.SetTrigger("shoot");
        }
    }

    // Called by ANIMATION EVENT 
    private void Shoot()
    {
        // Pick a projectile (first one, or random)
        GameObject projectile = GetProjectile();

        if (projectile != null)
        {
            // Position projectile at firePoint
            projectile.transform.position = firePoint.position;

            // Determine direction
            float direction = Mathf.Sign(transform.localScale.x);

            // Launch it
            projectile.GetComponent<Projectile>().Launch(direction);
        }
    }

    // Pick the first or a random projectile
    private GameObject GetProjectile()
    {
        // First available projectile (for pooling)
        foreach (var p in projectiles)
        {
            if (!p.activeInHierarchy)
                return p;
        }

        // If all are busy, use the first one (fallback)
        return projectiles.Length > 0 ? projectiles[0] : null;
    }
}
