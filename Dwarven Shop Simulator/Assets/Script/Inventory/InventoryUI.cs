using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;

    [Header("Assign slots manually")]
    public InventorySlotUI[] slotsUI;

    public int dragFromIndex = -1;

    void Start()
    {
        // Initialize slots
        for (int i = 0; i < slotsUI.Length; i++)
        {
            slotsUI[i].Initialize(inventory, this, i);
        }

        inventory.OnInventoryChanged += RefreshUI;

        RefreshUI();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slotsUI.Length; i++)
        {
            slotsUI[i].Refresh();
        }
    }

    public void MoveItem(int from, int to)
    {
        inventory.MoveItem(from, to);
        dragFromIndex = -1; // prevents reusing old index
    }
}