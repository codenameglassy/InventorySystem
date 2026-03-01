using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IDropHandler
{
    [Header("Gui Interface")]
    public Image icon;
    public TMP_Text amountText;
    public Image amountTextBg;

    public int slotIndex;

    private Inventory inventory;
    private InventoryUI inventoryUI; 
    public InventorySlot slot;

    public void Initialize(Inventory inventory, InventoryUI ui, int index)
    {
        this.inventory = inventory;
        this.inventoryUI = ui;
        this.slotIndex = index;
    }

    public void Refresh()
    {
        var slot = inventory.slots[slotIndex];

        if (slot.item == null)
        {
            icon.enabled = false;
            amountText.text = "";
            amountTextBg.enabled = false;
        }
        else
        {
            icon.enabled = true;
            icon.sprite = slot.item.icon;
            amountText.text = slot.amount.ToString();
            amountTextBg.enabled = true;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        var slot = inventory.slots[slotIndex];

        if (slot.item == null) return;

        // Update drag index
        inventoryUI.dragFromIndex = slotIndex;

        // Always update the icon with current slot's item
        DragItemUI.Instance.Show(slot.item.icon);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // position handled automatically in DragItemUI.Update()
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragItemUI.Instance.Hide();
        inventoryUI.dragFromIndex = -1;
    }

    // DROP TARGET
    public void OnDrop(PointerEventData eventData)
    {
        inventoryUI.MoveItem(inventoryUI.dragFromIndex, slotIndex);
    }

    private void OnDisable()
    {
        if (DragItemUI.Instance != null)
            DragItemUI.Instance.Hide();
    }
}