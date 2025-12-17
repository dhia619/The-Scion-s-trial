using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy Body")]
    [SerializeField] private Transform enemy;

    [Header("Movement")]
    [SerializeField] private float speed = 2f;

    private Vector3 initScale;
    private bool movingLeft = true;
    private float lockedYPosition; // Lock Y position to prevent floating

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration = 1f;
    private float idleTimer = 0f;

    [Header("Animation")]
    [SerializeField] private Animator anim;

    private EnemyController controller;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
        initScale = enemy.localScale;

        // Lock the Y position at start
        lockedYPosition = enemy.position.y;
    }

    private void OnDisable()
    {
        anim.SetBool("isMoving", false);
    }

    private void Update()
    {
        // Stop if dead or attacking or player is behind
        if (controller.IsDead || controller.IsAttacking || controller.PlayerBehind() || controller.PlayerInSight())
        {
            anim.SetBool("isMoving", false);
            return;
        }

        Patrol();
    }

    private void Patrol()
    {
        if (controller.PlayerInSight()) return;
        if (movingLeft)
        {
            if (enemy.position.x > leftEdge.position.x)
            {
                Move(-1);
                controller.SetFacingDirection(1);
            }
            else
                StartIdle();
        }
        else
        {
            if (enemy.position.x < rightEdge.position.x)
            {
                Move(1);
                controller.SetFacingDirection(1);
            }
            else
                StartIdle();
        }
    }

    private void StartIdle()
    {
        anim.SetBool("isMoving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            movingLeft = !movingLeft;
            idleTimer = 0f;
        }
    }

    private void Move(int direction)
    {
        idleTimer = 0f;
        anim.SetBool("isMoving", true);

        // Flip sprite
        enemy.localScale = new Vector3(
            Mathf.Abs(initScale.x) * direction,
            initScale.y,
            initScale.z
        );

        // Move only on X axis, force Y to stay locked
        enemy.position = new Vector3(
            enemy.position.x + (direction * speed * Time.deltaTime),
            lockedYPosition,
            enemy.position.z
        );
    }

    private void LateUpdate()
    {
        // Extra safety: force Y position every frame
        if (enemy != null && !controller.IsDead)
        {
            Vector3 pos = enemy.position;
            pos.y = lockedYPosition;
            enemy.position = pos;
        }
    }
    public void ForceTurn()
    {
        movingLeft = !movingLeft;
        idleTimer = 0f;
    }

}