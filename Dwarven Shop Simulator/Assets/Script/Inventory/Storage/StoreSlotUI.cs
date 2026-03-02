using UnityEngine.EventSystems;

public class StoreSlotUI : BaseSlotUI
{
    public void Initialize(InventorySlot inventorySlot, IDragSource source)
    {
        BaseInitialize(inventorySlot, source);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        HandleDrop(slot);
    }
}