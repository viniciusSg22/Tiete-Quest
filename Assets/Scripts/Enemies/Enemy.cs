using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float chaseSpeed = 3f;
    public float attackRange = 1.5f;
    public float jumpForce = 5f;

    public float detectionRadius = 8f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public int maxHealth = 3;
    private int currentHealth;

    private Rigidbody2D rb;
    private Transform targetPlayer;
    private bool isGrounded;
    private bool isAttacking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        StartCoroutine(UpdateTargetRoutine());
    }

    void Update()
    {
        CheckGround();

        if (targetPlayer == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, targetPlayer.position);

        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking)
                StartCoroutine(Attack());
        }
        else
        {
            ChasePlayer();
        }

        HandleJump();
    }

    void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        isGrounded = hit.collider != null;
    }

    void ChasePlayer()
    {
        Vector2 dir = (targetPlayer.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * chaseSpeed, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        if (!isGrounded) return;

        float verticalDiff = targetPlayer.position.y - transform.position.y;

        if (verticalDiff > 1f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        Debug.Log("Inimigo atacando!");

        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator UpdateTargetRoutine()
    {
        while (true)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            float minDist = detectionRadius;
            GameObject closest = null;

            foreach (GameObject player in players)
            {
                float dist = Vector2.Distance(transform.position, player.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = player;
                }
            }

            if (closest != null)
            {
                targetPlayer = closest.transform;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
