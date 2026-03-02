using UnityEngine;

public class StorageInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private InventoryPanel storagePanel;
    [SerializeField] private InventoryPanel playerInventoryPanel;

    public string PromptMessage => storagePanel.IsOpen ? "Press E to close" : "Press E to open";

    public void Interact()
    {
        if (storagePanel.IsOpen)
        {
            storagePanel.Close();
            playerInventoryPanel.Close();
            GameStateManager.Instance.OnStoreClosed();
        }
        else
        {
            storagePanel.Open();
            playerInventoryPanel.Open();
            GameStateManager.Instance.OnStoreOpened();
        }
    }
}