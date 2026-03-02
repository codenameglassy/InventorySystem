using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/CraftingRecipeDatabase")]
public class CraftingRecipeDatabase : ScriptableObject
{
    public List<CraftingRecipe> recipes = new List<CraftingRecipe>();

    public CraftingRecipe FindMatch(InventorySlot[] slots)
    {
        foreach (var recipe in recipes)
            if (recipe.Matches(slots)) return recipe;

        return null;
    }
}