using UnityEngine;

public class FlyingEyeShooter : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab;
    public Transform firePoint;

    public float shootRange = 6f;
    public float shootCooldown = 1.5f;
    private float cooldownTimer = 0f;

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        float dist = Vector2.Distance(transform.position, player.position);

        // Player NOT in sight
        if (dist > shootRange) return;

        // Player in sight AND ready to shoot
        if (cooldownTimer >= shootCooldown)
        {
            Shoot();
            cooldownTimer = 0f;
        }

        // Face player
        float dir = Mathf.Sign(player.position.x - transform.position.x);
        transform.localScale = new Vector3(dir, 1, 1);
    }

    void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Send projectile in eye's facing direction
        float direction = Mathf.Sign(transform.localScale.x);
        proj.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * 6f, 0);
    }
}
