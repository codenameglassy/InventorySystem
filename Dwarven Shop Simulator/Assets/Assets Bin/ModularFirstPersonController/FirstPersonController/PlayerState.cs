/// <summary>
/// Shared runtime state passed to all player components.
/// No MonoBehaviour — plain data container.
/// </summary>
public class PlayerState
{
    public bool IsGrounded { get; set; }
    public bool IsWalking { get; set; }
    public bool IsSprinting { get; set; }
    public bool IsCrouched { get; set; }
    public float CurrentSpeed { get; set; }
    public float SprintRemaining { get; set; }
}