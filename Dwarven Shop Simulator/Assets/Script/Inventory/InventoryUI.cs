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

        int fromIndex = inventory.slots.IndexOf(draggedSlot);
        int toIndex = inventory.slots.IndexOf(targetSlot);

        if (fromIndex >= 0 && toIndex >= 0)
            inventory.MoveItem(fromIndex, toIndex);

        ClearDraggedSlot();
    }
}