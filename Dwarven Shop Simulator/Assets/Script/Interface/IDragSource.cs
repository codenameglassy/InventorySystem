public interface IDragSource
{
    InventorySlot CurrentDraggedSlot { get; }
    void SetDraggedSlot(InventorySlot slot);
    void ClearDraggedSlot();
    void DropOnSlot(InventorySlot targetSlot);
}