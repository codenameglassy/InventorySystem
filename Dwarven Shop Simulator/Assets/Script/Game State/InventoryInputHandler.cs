using UnityEngine;

public class InventoryInputHandler : MonoBehaviour
{
    [System.Serializable]
    public class PanelBinding
    {
        public KeyCode key;
        public InventoryPanel panel;
    }

    public PanelBinding[] bindings;

    private void Update()
    {
        foreach (var binding in bindings)
            if (Input.GetKeyDown(binding.key))
                binding.panel.Toggle();
    }
}