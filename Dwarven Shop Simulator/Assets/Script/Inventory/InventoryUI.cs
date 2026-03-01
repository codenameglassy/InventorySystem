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

    public void DropOnSlot(InventorySlot targetSlot)
    {
        if (draggedSlot == null || targetSlot == null) return;

        int toIndex = inventory.slots.IndexOf(targetSlot);
        if (toIndex < 0) return;

        int fromIndex = inventory.slots.IndexOf(draggedSlot);

        if (fromIndex >= 0)
            inventory.MoveItem(fromIndex, toIndex);
        else
            inventory.MoveItemFromSlot(draggedSlot, toIndex);

        ClearDraggedSlot();
    }
}