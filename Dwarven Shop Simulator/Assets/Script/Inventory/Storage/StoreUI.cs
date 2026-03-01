using UnityEngine;

public class StoreUI : MonoBehaviour
{
    public Inventory storeInventory;
    public StoreSlotUI[] storeSlotsUI;
    public InventoryUI playerInventoryUI;

    private void Start()
    {
        for (int i = 0; i < storeSlotsUI.Length; i++)
        {
            storeSlotsUI[i].Initialize(storeInventory.slots[i], playerInventoryUI);
        }
    }
}