using UnityEngine;

public class PlayerController : MonoBehaviour, IGameStateObserver
{
    [Header("Components")]
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerCrouch playerCrouch;
    [SerializeField] private PlayerHeadBob playerHeadBob;
    [SerializeField] private PlayerSprintBar playerSprintBar;

    private PlayerState state;

    private void Awake()
    {
        state = new PlayerState();
        state.SprintRemaining = playerMovement.SprintDuration;

        playerLook.Initialize(state);
        playerMovement.Initialize(state);
        playerJump.Initialize(state);
        playerCrouch.Initialize(state);
        playerHeadBob.Initialize(state);
        playerSprintBar.Initialize(state);
    }

    private void Start()
    {
        GameStateManager.Instance.Register(this);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.Unregister(this);
    }

    public void OnStateChanged(GameState newState)
    {
        bool isNormal = newState == GameState.Normal;

        playerMovement.enabled = isNormal;
        playerJump.enabled = isNormal;
        playerCrouch.enabled = isNormal;
        playerHeadBob.enabled = isNormal;

        if (isNormal)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // Delay re-enabling camera by one frame to discard mouse delta
            StartCoroutine(EnableLookNextFrame());
        }
        else
        {
            playerLook.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private System.Collections.IEnumerator EnableLookNextFrame()
    {
        yield return null; // wait one frame for cursor lock to settle
        playerLook.enabled = true;
    }
}