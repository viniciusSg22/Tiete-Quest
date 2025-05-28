using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviourPun
{
    [Header("Moviment")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Vector2 moveInput;

    [Header("GroundCheck")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isJumping = false;
    private bool facingRight = true;

    [SerializeField]
    private GameObject OnScreenControls;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 5;

        if (photonView.IsMine) Instantiate(OnScreenControls);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine) return;
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine) return;
        if (context.performed && IsGrounded()) isJumping = true;
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            AudioManager.Instance.PlaySound(AudioManager.ESoundType.Jump);

            isJumping = false;
        }

        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (moveInput.x > 0 && !facingRight) Flip();
        else if (moveInput.x < 0 && facingRight) Flip();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}