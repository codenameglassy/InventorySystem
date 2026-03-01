using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public Inventory Inventory { get; private set; }
    public ItemData item;
    public int amount;

    public event Action OnSlotChanged;

    public InventorySlot() { } // for default constructor

    public InventorySlot(Inventory parentInventory)
    {
        Inventory = parentInventory;
    }

    public bool IsEmpty => item == null;

    public void Set(ItemData newItem, int newAmount)
    {
        item = newItem;
        amount = newAmount;
        NotifyChanged();
    }

    public void Clear()
    {
        item = null;
        amount = 0;
        NotifyChanged();
    }

    public void NotifyChanged()
    {
        OnSlotChanged?.Invoke();
    }
}