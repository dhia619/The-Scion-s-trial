using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float direction = 1;
    [SerializeField] private float speed = 5;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private bool hit;
    private float lifetime;
    void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (hit) return;

        // if the projectile didn't hit anything yet, let it move and increase lifetime
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /* 
         if the projectile hit smthg, then play the explode animation 
         and disable the collider to avoid triggering this event again.
        */
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");
        try
        { 
            collision.GetComponent<Animator>().SetTrigger("hurt");
        }
        catch
        {
            
        }
    }

    public void Launch(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction) 
        {
            localScaleX = -localScaleX;
        }
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    public void Desactivate()
    {
        gameObject.SetActive(false);
    }
}
