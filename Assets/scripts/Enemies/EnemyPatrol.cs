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
    }

    private void OnDisable()
    {
        anim.SetBool("isMoving", false);
    }

    private void Update()
    {
        // Stop if dead or attacking
        if (controller.IsDead || controller.IsAttacking)
        {
            anim.SetBool("isMoving", false);
            return;
        }

        Patrol();
    }

    private void Patrol()
    {
        if (movingLeft)
        {
            if (enemy.position.x > leftEdge.position.x)
                Move(-1);
            else
                StartIdle();
        }
        else
        {
            if (enemy.position.x < rightEdge.position.x)
                Move(1);
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

        // Move
        enemy.position += Vector3.right * direction * speed * Time.deltaTime;
    }
}
