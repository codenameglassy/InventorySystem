using UnityEngine.EventSystems;

public class CraftingSlotUI : BaseSlotUI
{
    public void Initialize(InventorySlot inventorySlot, IDragSource source)
    {
        BaseInitialize(inventorySlot, source);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        // Read from global drag state instead of local dragSource
        var draggedSlot = DragItemUI.Instance.DraggedSlot;
        var source = DragItemUI.Instance.DragSource;

        if (draggedSlot == null || draggedSlot == slot) return;

        var tempItem = slot.item;
        var tempAmount = slot.amount;

        slot.Set(draggedSlot.item, draggedSlot.amount);

        if (tempItem == null) draggedSlot.Clear();
        else draggedSlot.Set(tempItem, tempAmount);

        DragItemUI.Instance.Hide();
        source.ClearDraggedSlot();
    }
}