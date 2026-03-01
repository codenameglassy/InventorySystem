using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StoreSlotUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("UI Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Image amountTextBg;

    private InventorySlot storeSlot; // the slot this UI represents
    private Inventory storeInventory;
    private InventoryUI playerInventoryUI;

    public void Initialize(InventorySlot slot, Inventory store, InventoryUI playerUI)
    {
        storeSlot = slot;
        storeInventory = store;
        playerInventoryUI = playerUI;

        storeSlot.OnSlotChanged += Refresh;
        Refresh();
    }

    public void Refresh()
    {
        if (storeSlot.IsEmpty)
        {
            icon.enabled = false;
            amountText.text = "";
            amountTextBg.enabled = false;
        }
        else
        {
            icon.enabled = true;
            icon.sprite = storeSlot.item.icon;
            amountText.text = storeSlot.amount.ToString();
            amountTextBg.enabled = true;
        }
    }

    // ---------------- Drag logic ----------------
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (storeSlot.IsEmpty) return;

        playerInventoryUI.SetDraggedSlot(storeSlot);
        DragItemUI.Instance.Show(storeSlot.item.icon);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // DragItemUI follows mouse automatically
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragItemUI.Instance.Hide();
        playerInventoryUI.ClearDraggedSlot();
    }

    // ---------------- Drop logic ----------------
    public void OnDrop(PointerEventData eventData)
    {
        var draggedSlot = playerInventoryUI.CurrentDraggedSlot;
        if (draggedSlot == null || draggedSlot == storeSlot) return;

        // If dragged from player inventory → store slot
        if (storeSlot.IsEmpty)
        {
            storeSlot.Set(draggedSlot.item, draggedSlot.amount);
            draggedSlot.Clear();

            DragItemUI.Instance.Hide();
            playerInventoryUI.ClearDraggedSlot();
        }
        else
        {
            // Optional: swap items
            (storeSlot.item, draggedSlot.item) = (draggedSlot.item, storeSlot.item);
            (storeSlot.amount, draggedSlot.amount) = (draggedSlot.amount, storeSlot.amount);

            storeSlot.NotifyChanged();
            draggedSlot.NotifyChanged();
            DragItemUI.Instance.Hide();
            playerInventoryUI.ClearDraggedSlot();
        }
    }
}