using UnityEngine;

public class InventoryInputHandler : MonoBehaviour, IGameStateObserver
{
    [System.Serializable]
    public class PanelBinding
    {
        public KeyCode key;
        public InventoryPanel panel;
    }

    public PanelBinding[] bindings;

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
        // Only disable for Store — Normal and Inventory still work
        enabled = newState != GameState.Store;
    }

    private void Update()
    {
        foreach (var binding in bindings)
            if (Input.GetKeyDown(binding.key))
                binding.panel.Toggle();
    }
}