using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int direction = 1;
    [SerializeField] private float speed = 5;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private bool hit;
    void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");
        Debug.Log("exploded");
    }

    public void SetDirection(int _direction)
    {
        _direction = direction;
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
