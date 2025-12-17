using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movingSpeed = 2f;
    public int movingDirection = 1;
    public float jumpForce = 3.5f;
    public bool onGround = true;
    private Animator anim;
    private Rigidbody2D rb;
    
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (GetComponent<Health>().GetDead() || !canMove) 
        {
            rb.linearVelocityX = 0;
            anim.SetBool("isMoving", false);
            return;
        }
        
        anim.SetBool("isMoving", false);
        anim.SetBool("onGround", onGround);
        
        movingDirection = 0;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.localScale = new Vector3(9, 9, 1);
            movingDirection = 1;
            anim.SetBool("isMoving", true);
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            movingDirection = -1;
            transform.localScale = new Vector3(-9, 9, 1);
            anim.SetBool("isMoving", true);
        }
        rb.linearVelocityX = movingDirection * movingSpeed;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }

        if (rb.linearVelocityY < -1)
        {
            anim.SetBool("isFalling", true);
        }
    }

    public void Jump()
    {
        if (onGround && canMove)
        {
            rb.linearVelocityY = jumpForce;
            onGround = false;
            anim.SetTrigger("jump");
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {   
            onGround = true;
            anim.SetBool("isFalling", false);
        }
    }
    
    // Add only these two methods
    public void EnableMovement()
    {
        canMove = true;
    }
    
    public void DisableMovement()
    {
        canMove = false;
        rb.linearVelocityX = 0;
    }
}