using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float fov = 60f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 50f;
    [SerializeField] private bool invertCamera = false;

    [Header("Zoom")]
    [SerializeField] private bool enableZoom = true;
    [SerializeField] private bool holdToZoom = false;
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;
    [SerializeField] private float zoomFOV = 30f;
    [SerializeField] private float zoomStepTime = 5f;

    private PlayerState state;
    private float yaw;
    private float pitch;
    private bool isZoomed;

    // Sprint FOV is driven externally by PlayerMovement
    public Camera PlayerCamera => playerCamera;
    public float DefaultFOV => fov;
    public float ZoomStepTime => zoomStepTime;

    public void Initialize(PlayerState playerState)
    {
        state = playerState;
        playerCamera.fieldOfView = fov;
    }

    private void Update()
    {
        HandleRotation();
        HandleZoom();
    }

    private void HandleRotation()
    {
        yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch += mouseSensitivity * Input.GetAxis("Mouse Y") * (invertCamera ? 1 : -1);
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        transform.localEulerAngles = new Vector3(0, yaw, 0);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
    }

    private void HandleZoom()
    {
        if (!enableZoom || state.IsSprinting) return;

        if (!holdToZoom && Input.GetKeyDown(zoomKey))
            isZoomed = !isZoomed;

        if (holdToZoom)
        {
            if (Input.GetKeyDown(zoomKey)) isZoomed = true;
            if (Input.GetKeyUp(zoomKey)) isZoomed = false;
        }

        float targetFOV = isZoomed ? zoomFOV : fov;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, zoomStepTime * Time.deltaTime);
    }

    public void CancelZoom()
    {
        isZoomed = false;
    }
}