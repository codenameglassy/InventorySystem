using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private bool enableJump = true;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float groundCheckDistance = 0.75f;

    private PlayerState state;
    private Rigidbody rb;
    private PlayerCrouch playerCrouch;

    public void Initialize(PlayerState playerState)
    {
        state = playerState;
        rb = GetComponent<Rigidbody>();
        playerCrouch = GetComponent<PlayerCrouch>();
    }

    private void Update()
    {
        CheckGround();

        if (enableJump && Input.GetKeyDown(jumpKey) && state.IsGrounded)
            Jump();
    }

    private void CheckGround()
    {
        Vector3 origin = new Vector3(
            transform.position.x,
            transform.position.y - (transform.localScale.y * 0.5f),
            transform.position.z
        );

        state.IsGrounded = Physics.Raycast(origin, Vector3.down, groundCheckDistance);
        Debug.DrawRay(origin, Vector3.down * groundCheckDistance, Color.red);
    }

    private void Jump()
    {
        rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
        state.IsGrounded = false;

        // Uncrouch on jump if using toggle mode
        if (state.IsCrouched)
            playerCrouch?.Uncrouch();
    }
}