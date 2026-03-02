using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : BaseInventoryUI
{
    public Inventory inventory;
    public InventorySlotUI[] slotsUI;

    // InventoryUI.cs — change Start to Awake
    private void Awake()
    {
        int count = Mathf.Min(slotsUI.Length, inventory.slots.Count);
        for (int i = 0; i < count; i++)
            slotsUI[i].Initialize(inventory.slots[i], this);
    }

    protected override List<InventorySlot> GetSlots() => inventory.slots;
}