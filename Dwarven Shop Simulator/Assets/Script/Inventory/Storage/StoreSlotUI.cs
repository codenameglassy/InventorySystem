using UnityEngine.EventSystems;

public class StoreSlotUI : BaseSlotUI
{
    public void Initialize(InventorySlot inventorySlot, IDragSource source)
    {
        BaseInitialize(inventorySlot, source);
    }

    // Store items cannot be dragged out
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
}