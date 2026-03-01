using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int size = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        slots.Clear();
        for (int i = 0; i < size; i++)
            slots.Add(new InventorySlot());
    }

    public bool AddItem(ItemData item, int amount)
    {
        if (item == null) return false;
        int remaining = amount;

        // Try stacking
        if (item.stackable)
        {
            foreach (var slot in slots)
            {
                if (slot.item == item)
                {
                    int space = item.maxStack - slot.amount;
                    if (space > 0)
                    {
                        int add = Mathf.Min(space, remaining);
                        slot.Add(add);
                        remaining -= add;
                        if (remaining <= 0) return true;
                    }
                }
            }
        }

        // Place in empty slots
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                int add = Mathf.Min(item.maxStack, remaining);
                slot.Set(item, add);
                remaining -= add;
                if (remaining <= 0) return true;
            }
        }

        Debug.Log("Inventory full!");
        return false;
    }

    public void MoveItem(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex < 0 ||
            fromIndex >= slots.Count || toIndex >= slots.Count) return;

        var from = slots[fromIndex];
        var to = slots[toIndex];

        if (from.IsEmpty) return;

        // Stack if same item
        if (to.item == from.item && from.item.stackable)
        {
            int space = from.item.maxStack - to.amount;
            int moveAmount = Mathf.Min(space, from.amount);
            to.Add(moveAmount);
            from.Remove(moveAmount);
        }
        else
        {
            // Swap
            (from.item, to.item) = (to.item, from.item);
            (from.amount, to.amount) = (to.amount, from.amount);

            // Notify changes via public method
            from.NotifyChanged();
            to.NotifyChanged();
        }
    }
}