using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    [SerializeField] private bool enableCrouch = true;
    [SerializeField] private bool holdToCrouch = true;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private float crouchHeight = 0.75f;

    private PlayerState state;
    private Vector3 originalScale;

    public void Initialize(PlayerState playerState)
    {
        state = playerState;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (!enableCrouch) return;

        if (!holdToCrouch && Input.GetKeyDown(crouchKey))
            Toggle();

        if (holdToCrouch)
        {
            if (Input.GetKeyDown(crouchKey)) Crouch();
            if (Input.GetKeyUp(crouchKey)) Uncrouch();
        }
    }

    public void Toggle()
    {
        if (state.IsCrouched) Uncrouch();
        else Crouch();
    }

    public void Crouch()
    {
        if (state.IsCrouched) return;
        transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
        state.IsCrouched = true;
    }

    public void Uncrouch()
    {
        if (!state.IsCrouched) return;
        transform.localScale = originalScale;
        state.IsCrouched = false;
    }
}