using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/CraftingRecipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("3x3 Pattern (left to right, top to bottom)")]
    public CraftingRecipeSlot[] pattern = new CraftingRecipeSlot[9];

    [Header("Output")]
    public ItemData outputItem;
    public int outputAmount = 1;

    public bool Matches(InventorySlot[] slots)
    {
        if (slots.Length != 9) return false;

        for (int i = 0; i < 9; i++)
        {
            var expected = pattern[i];
            var actual = slots[i];

            // Both empty — fine
            if (expected.item == null && actual.IsEmpty) continue;

            // Item mismatch
            if (expected.item != actual.item) return false;

            // Not enough amount
            if (actual.amount < expected.amount) return false;
        }

        return true;
    }
}