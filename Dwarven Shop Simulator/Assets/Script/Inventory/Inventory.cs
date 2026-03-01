using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int size = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public Action OnInventoryChanged;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        slots.Clear();
        for (int i = 0; i < size; i++)
        {
            slots.Add(new InventorySlot(this));
        }
    }

    public bool AddItem(ItemData item, int amount)
    {
        // Stackable first
        foreach (var slot in slots)
        {
            if (slot.item == item && item.stackable)
            {
                slot.amount += amount;
                slot.NotifyChanged();
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        // Empty slot
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.Set(item, amount);
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        Debug.Log("Inventory Full");
        return false;
    }

    public void MoveItem(int fromIndex, int toIndex)
    {
        var from = slots[fromIndex];
        var to = slots[toIndex];

        (from.item, to.item) = (to.item, from.item);
        (from.amount, to.amount) = (to.amount, from.amount);

        from.NotifyChanged();
        to.NotifyChanged();
        OnInventoryChanged?.Invoke();
    }
}