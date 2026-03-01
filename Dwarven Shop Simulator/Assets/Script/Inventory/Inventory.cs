using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int size = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake() => Initialize();

    void Initialize()
    {
        slots.Clear();
        for (int i = 0; i < size; i++)
            slots.Add(new InventorySlot());
    }

    /// <summary>
    /// Adds item to inventory. Returns remaining amount that didn't fit (0 = full success).
    /// </summary>
    public int AddItem(ItemData item, int amount)
    {
        if (item == null) return amount;
        int remaining = amount;

        if (item.stackable)
        {
            foreach (var slot in slots)
            {
                if (slot.item != item) continue;
                int space = item.maxStack - slot.amount;
                if (space <= 0) continue;

                int add = Mathf.Min(space, remaining);
                slot.Add(add);
                remaining -= add;
                if (remaining <= 0) return 0;
            }
        }

        foreach (var slot in slots)
        {
            if (!slot.IsEmpty) continue;

            int add = Mathf.Min(item.maxStack, remaining);
            slot.Set(item, add);
            remaining -= add;
            if (remaining <= 0) return 0;
        }

        Debug.Log($"Inventory full! {remaining} of {item.itemName} could not be added.");
        return remaining;
    }

    /// <summary>
    /// Returns true if inventory contains at least the given amount of an item.
    /// </summary>
    public bool HasItem(ItemData item, int amount = 1)
    {
        int total = 0;
        foreach (var slot in slots)
        {
            if (slot.item == item) total += slot.amount;
            if (total >= amount) return true;
        }
        return false;
    }

    /// <summary>
    /// Removes a given amount of an item. Returns true if successful.
    /// </summary>
    public bool RemoveItem(ItemData item, int amount = 1)
    {
        if (!HasItem(item, amount)) return false;

        int remaining = amount;
        foreach (var slot in slots)
        {
            if (slot.item != item) continue;

            int remove = Mathf.Min(slot.amount, remaining);
            slot.Remove(remove);
            remaining -= remove;
            if (remaining <= 0) return true;
        }
        return true;
    }

    /// <summary>
    /// Moves item between two slots within this inventory.
    /// </summary>
    public void MoveItem(int fromIndex, int toIndex)
    {
        if (!IsValidIndex(fromIndex) || !IsValidIndex(toIndex)) return;

        var from = slots[fromIndex];
        var to = slots[toIndex];

        if (from.IsEmpty) return;

        if (to.item == from.item && from.item.stackable)
        {
            int space = from.item.maxStack - to.amount;
            int moveAmount = Mathf.Min(space, from.amount);
            to.Add(moveAmount);
            from.Remove(moveAmount);
        }
        else
        {
            (from.item, to.item) = (to.item, from.item);
            (from.amount, to.amount) = (to.amount, from.amount);
            from.NotifyChanged();
            to.NotifyChanged();
        }
    }

    /// <summary>
    /// Moves item from an external slot (e.g. store) into a slot in this inventory.
    /// </summary>
    public void MoveItemFromSlot(InventorySlot sourceSlot, int toIndex)
    {
        if (sourceSlot == null || sourceSlot.IsEmpty) return;
        if (!IsValidIndex(toIndex)) return;

        var target = slots[toIndex];

        if (target.item == sourceSlot.item && sourceSlot.item.stackable)
        {
            int space = sourceSlot.item.maxStack - target.amount;
            int moveAmount = Mathf.Min(space, sourceSlot.amount);
            target.Add(moveAmount);
            sourceSlot.Remove(moveAmount);
        }
        else if (target.IsEmpty)
        {
            target.Set(sourceSlot.item, sourceSlot.amount);
            sourceSlot.Clear();
        }
        else
        {
            var tempItem = target.item;
            var tempAmount = target.amount;
            target.Set(sourceSlot.item, sourceSlot.amount);
            sourceSlot.Set(tempItem, tempAmount);
        }
    }

    private bool IsValidIndex(int index) => index >= 0 && index < slots.Count;
}