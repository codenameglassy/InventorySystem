using UnityEngine;

public class StoreUI : MonoBehaviour, IDragSource
{
    public StoreInventory storeInventory;
    public InventoryUI playerInventoryUI;
    public StoreSlotUI[] storeSlotsUI;

    private InventorySlot draggedSlot;

    private void Start()
    {
        int count = Mathf.Min(storeSlotsUI.Length, storeInventory.slots.Count);
        for (int i = 0; i < count; i++)
            storeSlotsUI[i].Initialize(storeInventory.slots[i], this);
    }

    public void SetDraggedSlot(InventorySlot slot) => draggedSlot = slot;
    public void ClearDraggedSlot() => draggedSlot = null;

    public InventorySlot FindNextEmptySlot()
    {
        foreach (var slot in storeInventory.slots)
            if (slot.IsEmpty) return slot;
        return null;
    }

    public void CollectMatchingItems(InventorySlot targetSlot)
    {
        if (targetSlot == null || targetSlot.IsEmpty) return;

        foreach (var slot in storeInventory.slots)
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