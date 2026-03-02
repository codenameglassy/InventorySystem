using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Walking")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float maxVelocityChange = 10f;

    [Header("Sprint")]
    [SerializeField] private bool enableSprint = true;
    [SerializeField] private bool unlimitedSprint = false;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float sprintFOV = 80f;
    [SerializeField] private float sprintFOVStepTime = 10f;

    [Header("Sprint Duration")]
    [SerializeField] private float sprintDuration = 5f;
    [SerializeField] private float sprintCooldown = 0.5f;

    public float SprintDuration => sprintDuration;
    public float WalkSpeed => walkSpeed;

    private PlayerState state;
    private Rigidbody rb;
    private PlayerLook playerLook;

    private bool isSprintCooldown;
    private float sprintCooldownTimer;
    private const float SprintBarFadeInSpeed = 5f;
    private const float SprintBarFadeOutSpeed = 3f;

    public void Initialize(PlayerState playerState)
    {
        state = playerState;
        rb = GetComponent<Rigidbody>();
        playerLook = GetComponent<PlayerLook>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void Update()
    {
        HandleSprintRegen();
    }

    private void HandleMovement()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        bool wantsToSprint = enableSprint && Input.GetKey(sprintKey) && state.SprintRemaining > 0 && !isSprintCooldown;

        // ← Check actual horizontal velocity, not just input
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        state.IsWalking = horizontalVelocity.magnitude > 0.1f && state.IsGrounded;

        float targetSpeed = (wantsToSprint && state.IsWalking) ? sprintSpeed : walkSpeed;

        // Apply crouch speed reduction
        if (state.IsCrouched) targetSpeed *= 0.5f;

        state.IsSprinting = wantsToSprint && state.IsWalking;
        state.CurrentSpeed = targetSpeed;

        // FOV change while sprinting
        if (playerLook != null)
        {
            float targetFOV = state.IsSprinting ? sprintFOV : playerLook.DefaultFOV;
            playerLook.PlayerCamera.fieldOfView = Mathf.Lerp(
                playerLook.PlayerCamera.fieldOfView,
                targetFOV,
                sprintFOVStepTime * Time.deltaTime
            );

            if (state.IsSprinting) playerLook.CancelZoom();
        }

        Vector3 targetVelocity = transform.TransformDirection(input) * targetSpeed;
        Vector3 velocityChange = targetVelocity - rb.velocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void HandleSprintRegen()
    {
        if (!enableSprint || unlimitedSprint) return;

        if (state.IsSprinting)
        {
            state.SprintRemaining -= Time.deltaTime;
            if (state.SprintRemaining <= 0)
            {
                state.IsSprinting = false;
                isSprintCooldown = true;
                sprintCooldownTimer = sprintCooldown;
            }
        }
        else
        {
            state.SprintRemaining = Mathf.Clamp(state.SprintRemaining + Time.deltaTime, 0, sprintDuration);
        }

        if (isSprintCooldown)
        {
            sprintCooldownTimer -= Time.deltaTime;
            if (sprintCooldownTimer <= 0)
                isSprintCooldown = false;
        }
    }
}