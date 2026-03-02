using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventoryUI : MonoBehaviour, IDragSource, ISlotContainer
{
    protected InventorySlot draggedSlot;
    public InventorySlot CurrentDraggedSlot => draggedSlot;

    // IDragSource
    public void SetDraggedSlot(InventorySlot slot) => draggedSlot = slot;
    public void ClearDraggedSlot() => draggedSlot = null;

    // ISlotContainer — subclasses provide their slot list
    protected abstract List<InventorySlot> GetSlots();

    public InventorySlot FindNextEmptySlot()
    {
        foreach (var slot in GetSlots())
            if (slot.IsEmpty) return slot;
        return null;
    }

    public void CollectMatchingItems(InventorySlot targetSlot)
    {
        if (targetSlot == null || targetSlot.IsEmpty) return;

        foreach (var slot in GetSlots())
        {
            if (slot == targetSlot || slot.IsEmpty) continue;
            if (slot.item != targetSlot.item) continue;

            int space = targetSlot.item.maxStack - targetSlot.amount;
            if (space <= 0) break;

            int moveAmount = Mathf.Min(space, slot.amount);
            targetSlot.Add(moveAmount);
            slot.Remove(moveAmount);
        }
    }
}