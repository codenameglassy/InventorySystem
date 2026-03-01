using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemData item;
    public int amount;

    public event Action OnSlotChanged;

    public bool IsEmpty => item == null;

    public void Set(ItemData newItem, int newAmount)
    {
        item = newItem;
        amount = newAmount;
        NotifyChanged();
    }

    public void Add(int value)
    {
        if (item == null) return;
        amount += value;
        NotifyChanged();
    }

    public void Remove(int value)
    {
        if (item == null) return;
        amount -= value;
        if (amount <= 0) Clear();
        else NotifyChanged();
    }

    public void Clear()
    {
        item = null;
        amount = 0;
        NotifyChanged();
    }

    public void NotifyChanged() => OnSlotChanged?.Invoke();
}