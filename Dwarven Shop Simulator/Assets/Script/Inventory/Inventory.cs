using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int size = 20;

    public List<InventorySlot> slots = new List<InventorySlot>();

    public System.Action OnInventoryChanged;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        slots.Clear();

        for (int i = 0; i < size; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    public bool AddItem(ItemData item, int amount)
    {
        // try stack first
        foreach (var slot in slots)
        {
            if (slot.item == item && item.stackable)
            {
                slot.amount += amount;
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        // find empty slot
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

        OnInventoryChanged?.Invoke();
    }
}