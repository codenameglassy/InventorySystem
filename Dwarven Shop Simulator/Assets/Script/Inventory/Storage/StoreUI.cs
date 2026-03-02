using System.Collections.Generic;
using UnityEngine;

public class StoreUI : BaseInventoryUI
{
    public StoreInventory storeInventory;
    public StoreSlotUI[] storeSlotsUI;

    private void Awake() // ← was Start
    {
        int count = Mathf.Min(storeSlotsUI.Length, storeInventory.slots.Count);
        for (int i = 0; i < count; i++)
            storeSlotsUI[i].Initialize(storeInventory.slots[i], this);
    }

    protected override List<InventorySlot> GetSlots() => storeInventory.slots;
}