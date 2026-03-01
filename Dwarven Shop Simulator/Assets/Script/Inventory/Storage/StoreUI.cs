using UnityEngine;

public class StoreUI : MonoBehaviour
{
    public Inventory storeInventory;
    public InventoryUI playerInventoryUI; // reference player inventory UI
    public StoreSlotUI[] storeSlotsUI;

    private void Start()
    {
        for (int i = 0; i < storeSlotsUI.Length; i++)
        {
            storeSlotsUI[i].Initialize(storeInventory.slots[i], storeInventory, playerInventoryUI);
        }
    }
}