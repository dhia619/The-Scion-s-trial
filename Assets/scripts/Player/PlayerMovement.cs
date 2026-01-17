using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movingSpeed = 2f;
    public float jumpForce = 3.5f;
    public bool onGround = true;

    private Animator anim;
    private Rigidbody2D rb;
    private bool canMove = true;

    KeyCode moveRight;
    KeyCode moveLeft;
    KeyCode jumpKey;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        moveRight = BindingManager.Instance.GetControl("Move Right");
        moveLeft = BindingManager.Instance.GetControl("Move Left");
        jumpKey = BindingManager.Instance.GetControl("Jump");
    }

    void Update()
    {
        if (GetComponent<Health>().GetDead() || !canMove)
        {
            rb.linearVelocityX = 0;
            anim.SetBool("isMoving", false);
            return;
        }

        anim.SetBool("onGround", onGround);

        // -------- MOVE (KEYBOARD + STICK + DPAD) --------
        float axis = Input.GetAxisRaw("Horizontal"); // stick + dpad

        bool rightKey =
            Input.GetKey(moveRight) ||
            Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.RightArrow);

        bool leftKey =
            Input.GetKey(moveLeft) ||
            Input.GetKey(KeyCode.Q) ||
            Input.GetKey(KeyCode.LeftArrow);

        int dir = 0;

        if (axis > 0.2f || rightKey)
            dir = 1;
        else if (axis < -0.2f || leftKey)
            dir = -1;

        rb.linearVelocityX = dir * movingSpeed;

        anim.SetBool("isMoving", dir != 0);

        if (dir == 1)
            transform.localScale = new Vector3(9, 9, 1);
        else if (dir == -1)
            transform.localScale = new Vector3(-9, 9, 1);

        // -------- JUMP --------
        bool jumpPressed =
            Input.GetKeyDown(jumpKey) ||
            Input.GetKeyDown(KeyCode.Z) ||
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetButtonDown("Jump");

        if (jumpPressed)
            Jump();

        if (rb.linearVelocityY < -1)
            anim.SetBool("isFalling", true);
    }

    void Jump()
    {
        if (!onGround || !canMove) return;

        rb.linearVelocityY = jumpForce;
        onGround = false;
        anim.SetTrigger("jump");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            anim.SetBool("isFalling", false);
        }
    }

    public void EnableMovement() => canMove = true;

    public void DisableMovement()
    {
        canMove = false;
        rb.linearVelocityX = 0;
    }
}
