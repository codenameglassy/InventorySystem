using UnityEngine;

public class CameraController : MonoBehaviour, IGameStateObserver
{
    [SerializeField] private MonoBehaviour cameraMovementComponent;

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
        cameraMovementComponent.enabled = newState == GameState.Normal;
    }
}
