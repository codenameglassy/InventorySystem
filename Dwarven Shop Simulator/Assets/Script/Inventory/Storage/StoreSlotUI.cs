using UnityEngine.EventSystems;

public class StoreSlotUI : BaseSlotUI
{
    public void Initialize(InventorySlot inventorySlot, IDragSource source)
    {
        BaseInitialize(inventorySlot, source);
    }

    // Custom drop — moves from player into store slot
    public override void OnDrop(PointerEventData eventData)
    {
        var draggedSlot = dragSource.CurrentDraggedSlot;
        if (draggedSlot == null || draggedSlot == slot) return;

        if (slot.IsEmpty)
        {
            slot.Set(draggedSlot.item, draggedSlot.amount);
            draggedSlot.Clear();
        }
        else
        {
            var tempItem = slot.item;
            var tempAmount = slot.amount;
            slot.Set(draggedSlot.item, draggedSlot.amount);
            draggedSlot.Set(tempItem, tempAmount);
        }

        DragItemUI.Instance.Hide();
        dragSource.ClearDraggedSlot();
    }
}