// CraftingSlotUI.cs
using UnityEngine.EventSystems;
public class CraftingSlotUI : BaseSlotUI
{
    public void Initialize(InventorySlot inventorySlot, BaseInventoryUI source)
    {
        BaseInitialize(inventorySlot, source);
    }

    public override void OnDrop(PointerEventData eventData) => HandleDrop(slot);
}
