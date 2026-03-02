using UnityEngine;

public class InventoryPanel : MonoBehaviour, IOpenable
{
    public bool IsOpen { get; private set; }

    private void Awake()
    {
        // Force closed before anything else runs
        IsOpen = false;
        gameObject.SetActive(false);
    }

    public void Open()
    {
        if (IsOpen) return;
        IsOpen = true;
        gameObject.SetActive(true);
        GameStateManager.Instance.OnInventoryOpened();
    }

    public void Close()
    {
        if (!IsOpen) return;
        IsOpen = false;
        gameObject.SetActive(false);
        GameStateManager.Instance.OnInventoryClosed();
    }

    public void Toggle()
    {
        if (IsOpen) Close();
        else Open();
    }

    private void OnDestroy()
    {
        if (IsOpen) GameStateManager.Instance.OnInventoryClosed();
    }
}