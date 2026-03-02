using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingUI : MonoBehaviour, IDragSource
{
    [Header("References")]
    public CraftingGrid craftingGrid;
    public CraftingSlotUI[] slotUIs;
    public CraftingRecipeDatabase recipeDatabase;
    public Inventory playerInventory;

    [Header("Output Display")]
    public Image outputIcon;
    public TMP_Text outputAmountText;
    public Button craftButton;

    private InventorySlot draggedSlot;
    public InventorySlot CurrentDraggedSlot => draggedSlot;
    private CraftingRecipe currentMatch;

    private void Start()
    {
        int count = Mathf.Min(slotUIs.Length, craftingGrid.slots.Count);
        for (int i = 0; i < count; i++)
            slotUIs[i].Initialize(craftingGrid.slots[i], this);

        craftingGrid.OnGridChanged += CheckRecipe;
        craftButton.onClick.AddListener(OnCraftClicked);
        ClearOutput();
    }

    private void CheckRecipe()
    {
        currentMatch = recipeDatabase.FindMatch(craftingGrid.GetSlots());

        if (currentMatch != null)
        {
            outputIcon.enabled = true;
            outputIcon.sprite = currentMatch.outputItem.icon;
            outputAmountText.text = currentMatch.outputAmount.ToString();
            craftButton.interactable = true;
        }
        else
        {
            ClearOutput();
        }
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
        ClearOutput();
    }

    private void ClearOutput()
    {
        outputIcon.enabled = false;
        outputAmountText.text = "";
        craftButton.interactable = false;
        currentMatch = null;
    }

    public void SetDraggedSlot(InventorySlot slot) => draggedSlot = slot;
    public void ClearDraggedSlot() => draggedSlot = null;

    public InventorySlot FindNextEmptySlot()
    {
        foreach (var s in craftingGrid.slots)
            if (s.IsEmpty) return s;
        return null;
    }

    public void CollectMatchingItems(InventorySlot targetSlot)
    {
        if (targetSlot == null || targetSlot.IsEmpty) return;

        foreach (var slot in craftingGrid.slots)
        {
            if (slot == targetSlot || slot.IsEmpty) continue;
            if (slot.item != targetSlot.item) continue;

            int space = targetSlot.item.maxStack - targetSlot.amount;
            if (space <= 0) break;

            int moveAmount = Mathf.Min(space, slot.amount);
            targetSlot.Add(moveAmount);
            slot.Remove(moveAmount);
        }
    }
}