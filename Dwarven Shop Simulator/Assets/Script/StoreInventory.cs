using System.Collections.Generic;
using UnityEngine;

public class StoreInventory : MonoBehaviour
{
    public int size = 10;
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        slots.Clear();
        for (int i = 0; i < size; i++)
            slots.Add(new InventorySlot());
    }
}