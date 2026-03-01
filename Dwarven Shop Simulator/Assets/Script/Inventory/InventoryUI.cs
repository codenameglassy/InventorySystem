using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public InventorySlotUI[] slotsUI;

    private InventorySlot draggedSlot;
    private Inventory sourceInventory;

    public InventorySlot CurrentDraggedSlot => draggedSlot;

    public void SetDraggedSlot(InventorySlot slot)
    {
        draggedSlot = slot;
        sourceInventory = slot.Inventory;
    }

    public void ClearDraggedSlot()
    {
        draggedSlot = null;
        sourceInventory = null;
    }

    public void DropOnSlot(InventorySlot targetSlot)
    {
        if (draggedSlot == null || targetSlot == null) return;

        // Different inventories
        if (sourceInventory != targetSlot.Inventory)
        {
            if (targetSlot.IsEmpty)
            {
                targetSlot.Set(draggedSlot.item, draggedSlot.amount);
                draggedSlot.Clear();
            }
            else
            {
                var tempItem = targetSlot.item;
                var tempAmount = targetSlot.amount;

                targetSlot.Set(draggedSlot.item, draggedSlot.amount);
                draggedSlot.Set(tempItem, tempAmount);
            }
        }
        else
        {
            // Same inventory → use MoveItem
            int fromIndex = sourceInventory.slots.IndexOf(draggedSlot);
            int toIndex = inventory.slots.IndexOf(targetSlot);
            sourceInventory.MoveItem(fromIndex, toIndex);
        }

        ClearDraggedSlot();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slotsUI.Length; i++)
            slotsUI[i].Refresh();
    }
}