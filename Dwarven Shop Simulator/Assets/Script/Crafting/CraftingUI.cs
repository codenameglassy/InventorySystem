using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : BaseInventoryUI
{
    [Header("References")]
    public CraftingGrid craftingGrid;
    public CraftingSlotUI[] slotUIs;
    public CraftingRecipeDatabase recipeDatabase;
    public Inventory playerInventory;
    public CraftingOutputUI outputUI;

    private CraftingRecipe currentMatch;

    private void Start()
    {
        int count = Mathf.Min(slotUIs.Length, craftingGrid.slots.Count);
        for (int i = 0; i < count; i++)
            slotUIs[i].Initialize(craftingGrid.slots[i], this);

        craftingGrid.OnGridChanged += CheckRecipe;
        outputUI.SetCraftAction(OnCraftClicked);
        outputUI.ClearOutput();
    }

    private void CheckRecipe()
    {
        currentMatch = recipeDatabase.FindMatch(craftingGrid.GetSlots());

        if (currentMatch != null) outputUI.ShowOutput(currentMatch);
        else outputUI.ClearOutput();
    }

    private void OnCraftClicked()
    {
        if (currentMatch == null) return;

        int remaining = playerInventory.AddItem(currentMatch.outputItem, currentMatch.outputAmount);
        if (remaining > 0)
        {
            Debug.Log("Not enough inventory space to craft!");
            return;
        }

        craftingGrid.ConsumeIngredients(currentMatch);
        outputUI.ClearOutput();
        currentMatch = null;
    }

    protected override List<InventorySlot> GetSlots() => craftingGrid.slots;
}