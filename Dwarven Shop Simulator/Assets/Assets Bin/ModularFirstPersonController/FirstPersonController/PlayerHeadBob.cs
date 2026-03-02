using UnityEngine;

public class PlayerHeadBob : MonoBehaviour
{
    [SerializeField] private bool enableHeadBob = true;
    [SerializeField] private Transform joint;
    [SerializeField] private float bobSpeed = 10f;
    [SerializeField] private Vector3 bobAmount = new Vector3(0.15f, 0.05f, 0f);

    private PlayerState state;
    private PlayerMovement playerMovement;
    private Vector3 jointOriginalPos;
    private float timer;

    public void Initialize(PlayerState playerState)
    {
        state = playerState;
        playerMovement = GetComponent<PlayerMovement>();
        jointOriginalPos = joint.localPosition;
    }

    private void Update()
    {
        if (!enableHeadBob) return;

        if (state.IsWalking)
            Bob();
        else
            ResetBob();
    }

    private void Bob()
    {
        float speed = bobSpeed;
        if (state.IsSprinting) speed += playerMovement.WalkSpeed;
        if (state.IsCrouched) speed *= 0.5f;

        timer += Time.deltaTime * speed;
        joint.localPosition = new Vector3(
            jointOriginalPos.x + Mathf.Cos(timer) * bobAmount.x,   // ← Cos for smooth side sway
            jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y,   // ← Sin for up/down
            jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z
        );
    }

    private void ResetBob()
    {
        timer = 0;
        joint.localPosition = Vector3.Lerp(joint.localPosition, jointOriginalPos, Time.deltaTime * bobSpeed);
    }
}