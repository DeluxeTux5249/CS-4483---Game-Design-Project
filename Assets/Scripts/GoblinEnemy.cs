using UnityEngine;
using UnityEngine.XR;

public class GoblinEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;
    private int facingDirection = 1; // 1 for right, -1 for left
    private Animator animator;
    public float attackRange = 1f;
    public GoblinEnemyState enemyState;
    public float attackCooldown = 1f;
    public float attackTimer = 0.5f;
    public float playerDetectionRange = 5f;
    public Transform detectionPoint;
    public LayerMask playerLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ChangeState(GoblinEnemyState.Idle);
    }

    void Update()
    {
        CheckForPlayer();

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (enemyState == GoblinEnemyState.Run)
        {
            Chase();
        }
        else if (enemyState == GoblinEnemyState.Attack_Right ||
                 enemyState == GoblinEnemyState.Attack_Down ||
                 enemyState == GoblinEnemyState.Attack_Up)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Chase()
    {
        if (player.position.x > transform.position.x && facingDirection == -1)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * 3.5f;
    }

    void Flip()
    {
        facingDirection *= -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void CheckForPlayer()
    {
        Collider2D[] hitColliders =
            Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectionRange, playerLayer);

        if (hitColliders.Length > 0)
        {
            player = hitColliders[0].transform;

            float dist = Vector2.Distance(transform.position, player.position);

            if (dist <= attackRange && attackTimer <= 0)
            {
                attackTimer = attackCooldown;

                // choose attack direction
                if (player.position.y < transform.position.y - 0.2f)
                {
                    ChangeState(GoblinEnemyState.Attack_Down);
                }
                else if (player.position.y > transform.position.y + 0.2f)
                {
                    ChangeState(GoblinEnemyState.Attack_Up);
                }
                else
                {
                    ChangeState(GoblinEnemyState.Attack_Right);
                }
            }
            else if (dist > attackRange)
            {
                ChangeState(GoblinEnemyState.Run);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(GoblinEnemyState.Idle);
        }
    }

    void ChangeState(GoblinEnemyState state)
    {
        if (enemyState == GoblinEnemyState.Idle)
            animator.SetBool("isIdle", false);
        else if (enemyState == GoblinEnemyState.Run)
            animator.SetBool("isChasing", false);
        else if (enemyState == GoblinEnemyState.Attack_Right)
            animator.SetBool("isAttacking", false);
        else if (enemyState == GoblinEnemyState.Attack_Down)
            animator.SetBool("isAttackDown", false);
        else if (enemyState == GoblinEnemyState.Attack_Up)
            animator.SetBool("isAttackUp", false);

        enemyState = state;

        if (enemyState == GoblinEnemyState.Idle)
            animator.SetBool("isIdle", true);
        else if (enemyState == GoblinEnemyState.Run)
            animator.SetBool("isChasing", true);
        else if (enemyState == GoblinEnemyState.Attack_Right)
            animator.SetBool("isAttacking", true);
        else if (enemyState == GoblinEnemyState.Attack_Down)
            animator.SetBool("isAttackDown", true);
        else if (enemyState == GoblinEnemyState.Attack_Up)
            animator.SetBool("isAttackUp", true);
    }
}

public enum GoblinEnemyState
{
    Idle,
    Run,
    Attack_Right,
    Attack_Down,
    Attack_Up
}