public interface IDragSource
{
    void SetDraggedSlot(InventorySlot slot);
    void ClearDraggedSlot();
    InventorySlot FindNextEmptySlot();
    void CollectMatchingItems(InventorySlot targetSlot); // ← new
}