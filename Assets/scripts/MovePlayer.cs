using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MovePlayer : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int moveDirection = 1;
    public float jumpForce = 3.5f;
    public bool on_ground = true;
    private Animator runAnimation;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        runAnimation = GetComponent<Animator>();
    }

    void Update()
    {
        runAnimation.SetBool("run", false);
        runAnimation.SetBool("landed_on_ground", on_ground);
        // --- Movement ---
        moveDirection = 0;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.localScale = new Vector3(1, 1, 1);
            moveDirection = 1;
            runAnimation.SetBool("run", true);
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = -1;
            transform.localScale = new Vector3(-1, 1, 1);
            runAnimation.SetBool("run", true);
        }
        rb.linearVelocityX = moveDirection * moveSpeed;

        // --- Jump ---
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }

    }

    public void Jump()
    {
        rb.linearVelocityY = jumpForce;
        on_ground = false;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            on_ground = true;
        }
    }
}