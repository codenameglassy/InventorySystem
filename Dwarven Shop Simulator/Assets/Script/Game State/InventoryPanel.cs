using UnityEngine;

public class InventoryPanel : MonoBehaviour, IOpenable
{
    public bool IsOpen { get; private set; }

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

    // Safety — if destroyed while open, notify manager
    private void OnDestroy()
    {
        if (IsOpen) GameStateManager.Instance.OnInventoryClosed();
    }
}