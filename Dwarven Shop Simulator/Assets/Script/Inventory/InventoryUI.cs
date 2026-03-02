using UnityEngine;

public class InventoryUI : MonoBehaviour, IDragSource
{
    public Inventory inventory;
    public InventorySlotUI[] slotsUI;

    private InventorySlot draggedSlot;
    public InventorySlot CurrentDraggedSlot => draggedSlot;

    private void Start()
    {
        int count = Mathf.Min(slotsUI.Length, inventory.slots.Count);
        for (int i = 0; i < count; i++)
            slotsUI[i].Initialize(inventory.slots[i], this);
    }

    public void SetDraggedSlot(InventorySlot slot) => draggedSlot = slot;
    public void ClearDraggedSlot() => draggedSlot = null;
}