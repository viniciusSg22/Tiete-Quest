using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Enemy : MonoBehaviourPun
{
    [Header("Movimento")]
    public float moveSpeed = 2f;
    public float chaseSpeed = 3f;
    public float attackRange = 1.5f;
    public float jumpForce = 5f;

    [Header("Detecção")]
    public float detectionRadius = 8f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;
    public Transform groundAheadCheck;

    [Header("Estado")]
    public bool patrolEnabled = true;

    [Header("Vida")]
    public int maxHealth = 3;
    private int currentHealth;

    private Rigidbody2D rb;
    private Transform targetPlayer;
    private bool isGrounded;
    private bool isAttacking;
    private bool isFacingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        if (PhotonNetwork.IsMasterClient) StartCoroutine(UpdateTargetRoutine());
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        CheckGround();

        if (targetPlayer == null)
        {
            Patrol();
            return;
        }

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

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Patrol()
    {
        if (!patrolEnabled) return;

        float dir = isFacingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        if (IsHittingWall() || !HasGroundAhead())
            Flip();
    }

    private void ChasePlayer()
    {
        Vector2 direction = (targetPlayer.position - transform.position).normalized;

        if (direction.x > 0 && !isFacingRight) Flip();
        else if (direction.x < 0 && isFacingRight) Flip();

        rb.linearVelocity = new Vector2(direction.x * chaseSpeed, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        if (!isGrounded) return;

        float verticalDiff = targetPlayer.position.y - transform.position.y;

        if (verticalDiff > 1f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private bool IsHittingWall()
    {
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, direction, wallCheckDistance, groundLayer);
        return hit.collider != null;
    }

    private bool HasGroundAhead()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundAheadCheck.position, Vector2.down, 1f, groundLayer);
        return hit.collider != null;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        Debug.Log("Inimigo atacando!");

        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
            PhotonNetwork.Destroy(gameObject);
    }

    private IEnumerator UpdateTargetRoutine()
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

            targetPlayer = closest != null ? closest.transform : null;

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (isFacingRight ? Vector3.right : Vector3.left) * wallCheckDistance);
        }

        if (groundAheadCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(groundAheadCheck.position, groundAheadCheck.position + Vector3.down * 1f);
        }
    }
}
