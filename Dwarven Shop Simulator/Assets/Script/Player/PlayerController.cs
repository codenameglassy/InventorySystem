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

        // Lock cursor on start
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

        playerLook.enabled = isNormal;
        playerMovement.enabled = isNormal;
        playerJump.enabled = isNormal;
        playerCrouch.enabled = isNormal;
        playerHeadBob.enabled = isNormal;

        Cursor.lockState = isNormal ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isNormal;
    }
}