using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("UI Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Image amountTextBg;

    private InventorySlot slot;
    private InventoryUI inventoryUI;

    // Public getter for inventoryUI access
    public InventorySlot Slot => slot;

    public void Initialize(InventorySlot slot, InventoryUI ui)
    {
        this.slot = slot;
        this.inventoryUI = ui;

        slot.OnSlotChanged += Refresh;
        Refresh();
    }

    public void Refresh()
    {
        if (slot.IsEmpty)
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
        if (slot.IsEmpty) return;
        inventoryUI.SetDraggedSlot(slot);
        DragItemUI.Instance.Show(slot.item.icon);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // DragItemUI follows mouse automatically
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragItemUI.Instance.Hide();
        inventoryUI.ClearDraggedSlot();
    }

    public void OnDrop(PointerEventData eventData)
    {
        inventoryUI.DropOnSlot(slot);
    }
}