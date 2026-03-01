using UnityEngine;
using UnityEngine.EventSystems;

public class SellSlotUI : MonoBehaviour, IDropHandler
{
    public InventoryUI inventoryUI; // reference your inventory UI

    public void OnDrop(PointerEventData eventData)
    {
        if (inventoryUI.CurrentDraggedSlot == null) return;

        SellItem(inventoryUI.CurrentDraggedSlot);

        // Remove item from inventory
        inventoryUI.CurrentDraggedSlot.Clear();

        // Hide drag icon and reset
        DragItemUI.Instance.Hide();
        inventoryUI.ClearDraggedSlot();
    }

    void SellItem(InventorySlot slot)
    {
        Debug.Log($"Selling {slot.item.itemName} x{slot.amount}");
        // TODO: Add gold addition or other sell logic here
    }
}