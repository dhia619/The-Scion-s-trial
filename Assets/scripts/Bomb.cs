using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private float fuseTime = 2f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private LayerMask damageLayers;

    [Header("Effects")]
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private ParticleSystem explosionEffect;

    private Animator anim;
    private Rigidbody2D rb;
    private bool hasExploded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(Fuse());
    }

    private IEnumerator Fuse()
    {
        yield return new WaitForSeconds(fuseTime);
        Explode();
    }

    public void ThrowBomb(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void Explode()
    {
        if (hasExploded) return;
        anim.SetTrigger("explode");
        hasExploded = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    public void ActualExplosion()
    {
        // Sound
        if (explosionSound != null)
            SoundManager.instance.PlaySound(explosionSound);

        // Particle effect
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);


        // Damage
        var hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageLayers);
        foreach (var hit in hits)
        {
            Health h = hit.GetComponent<Health>();
            if (h != null)
                h.TakeDamage(damage);
        }
    }

    public void Desactivate()
    {
        Destroy(gameObject);
    }
}
