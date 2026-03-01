using UnityEngine;

public class StoreUI : MonoBehaviour
{
    public Inventory storeInventory;
    public InventoryUI playerInventoryUI;
    public StoreSlotUI[] storeSlotsUI;

    private void Start()
    {
        int count = Mathf.Min(storeSlotsUI.Length, storeInventory.slots.Count);
        for (int i = 0; i < count; i++)
            storeSlotsUI[i].Initialize(storeInventory.slots[i], playerInventoryUI);
    }
}