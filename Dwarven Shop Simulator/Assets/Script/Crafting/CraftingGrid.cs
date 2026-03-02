using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingGrid : MonoBehaviour
{
    public const int GridSize = 9;
    public List<InventorySlot> slots = new List<InventorySlot>();

    public event Action OnGridChanged;

    private void Awake()
    {
        slots.Clear();
        for (int i = 0; i < GridSize; i++)
        {
            var slot = new InventorySlot();
            slot.OnSlotChanged += () => OnGridChanged?.Invoke();
            slots.Add(slot);
        }
    }

    public void ConsumeIngredients(CraftingRecipe recipe)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            int requiredAmount = recipe.pattern[i].item != null ? recipe.pattern[i].amount : 0;
            if (requiredAmount > 0)
                slots[i].Remove(requiredAmount); // removes exact required amount
        }
    }

    public InventorySlot[] GetSlots() => slots.ToArray();
}