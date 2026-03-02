public interface ISlotContainer
{
    InventorySlot FindNextEmptySlot();
    void CollectMatchingItems(InventorySlot targetSlot);
}