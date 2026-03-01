using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public InventorySlotUI[] slotsUI;

    private InventorySlot draggedSlot;
    public InventorySlot CurrentDraggedSlot => draggedSlot;

    private void Start()
    {
        for (int i = 0; i < slotsUI.Length; i++)
        {
            slotsUI[i].Initialize(inventory.slots[i], this);
        }
    }

    public void SetDraggedSlot(InventorySlot slot)
    {
        draggedSlot = slot;
    }

    public void ClearDraggedSlot()
    {
        draggedSlot = null;
    }

    public void DropOnSlot(InventorySlot targetSlot)
    {
        if (draggedSlot == null || targetSlot == null) return;

        int toIndex = inventory.slots.IndexOf(targetSlot);
        if (toIndex < 0) return;

        int fromIndex = inventory.slots.IndexOf(draggedSlot);

        if (fromIndex >= 0)
        {
            // Same inventory — normal move
            inventory.MoveItem(fromIndex, toIndex);
        }
        else
        {
            // Cross-inventory (e.g. store → player)
            inventory.MoveItemFromSlot(draggedSlot, toIndex);
        }

        ClearDraggedSlot();
    }
}