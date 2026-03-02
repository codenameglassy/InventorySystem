using UnityEngine;

public class StorageInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private InventoryPanel storagePanel;
    [SerializeField] private InventoryPanel playerInventoryPanel;
    [SerializeField] private string promptMessage = "Press E to open storage";

    public string PromptMessage => promptMessage;

    public void Interact()
    {
        storagePanel.Open();
        playerInventoryPanel.Open();
    }
}