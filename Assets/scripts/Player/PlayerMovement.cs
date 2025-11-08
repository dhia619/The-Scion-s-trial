using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int moveDirection = 1;
    public float jumpForce = 3.5f;
    public bool on_ground = true;
    public bool transformed;
    private Animator MoveAnimation;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MoveAnimation = GetComponent<Animator>();
    }

    void Update()
    {
        MoveAnimation.SetBool("run", false);
        MoveAnimation.SetBool("landed_on_ground", on_ground);
        // --- Movement ---
        moveDirection = 0;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.localScale = new Vector3(2, 2, 1);
            moveDirection = 1;
            MoveAnimation.SetBool("run", true);
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = -1;
            transform.localScale = new Vector3(-2, 2, 1);
            MoveAnimation.SetBool("run", true);
        }
        rb.linearVelocityX = moveDirection * moveSpeed;

        // --- Jump ---
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }

        if (rb.linearVelocityY < -1)
        {
            MoveAnimation.SetTrigger("fall");
        }
    }

    public void Jump()
    {
        if (on_ground)
        {
            rb.linearVelocityY = jumpForce;
            on_ground = false;
            MoveAnimation.SetTrigger("jump");
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            on_ground = true;
        }
    }
}