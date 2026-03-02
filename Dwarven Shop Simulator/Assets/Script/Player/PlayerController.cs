using UnityEngine;

public class PlayerController : MonoBehaviour, IGameStateObserver
{
    [Header("Components")]
    [SerializeField] private MonoBehaviour movementComponent;
    [SerializeField] private MonoBehaviour interactionComponent;

    private void Start()
    {
        GameStateManager.Instance.Register(this);
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.Unregister(this);
    }

    public void OnStateChanged(GameState newState)
    {
        bool isNormal = newState == GameState.Normal;
        movementComponent.enabled = isNormal;
        interactionComponent.enabled = isNormal;
    }
}