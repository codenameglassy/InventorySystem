using UnityEngine.EventSystems;
using UnityEngine;

public class CraftingSlotUI : BaseSlotUI
{
    public void Initialize(InventorySlot inventorySlot, IDragSource source)
    {
        BaseInitialize(inventorySlot, source);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        var draggedSlot = DragItemUI.Instance.DraggedSlot;
        var source = DragItemUI.Instance.DragSource;

        if (draggedSlot == null || draggedSlot == slot) return;

        // Stack if same item and stackable
        if (slot.item == draggedSlot.item && slot.item.stackable)
        {
            int space = slot.item.maxStack - slot.amount;
            int moveAmount = Mathf.Min(space, draggedSlot.amount);

            if (moveAmount > 0)
            {
                slot.Add(moveAmount);
                draggedSlot.Remove(moveAmount);
            }
        }
        else
        {
            // Swap
            var tempItem = slot.item;
            var tempAmount = slot.amount;

            slot.Set(draggedSlot.item, draggedSlot.amount);

            if (tempItem == null) draggedSlot.Clear();
            else draggedSlot.Set(tempItem, tempAmount);
        }

        DragItemUI.Instance.Hide();
        source?.ClearDraggedSlot();
    }
}