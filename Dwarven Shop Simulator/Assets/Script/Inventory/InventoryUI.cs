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

    public InventorySlot FindNextEmptySlot()
    {
        foreach (var slot in inventory.slots)
            if (slot.IsEmpty) return slot;
        return null;
    }

    public void CollectMatchingItems(InventorySlot targetSlot)
    {
        if (targetSlot == null || targetSlot.IsEmpty) return;

        foreach (var slot in inventory.slots)
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