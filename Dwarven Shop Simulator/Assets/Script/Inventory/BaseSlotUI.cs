using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public abstract class BaseSlotUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler,
    IPointerClickHandler, IPointerDownHandler
{
    [Header("UI Elements")]
    [SerializeField] protected Image icon;
    [SerializeField] protected TMP_Text amountText;
    [SerializeField] protected Image amountTextBg;

    protected InventorySlot slot;
    protected IDragSource dragSource;
    protected ISlotContainer slotContainer;

    private float lastClickTime;
    private const float DoubleClickThreshold = 0.3f;

    protected void BaseInitialize(InventorySlot inventorySlot, BaseInventoryUI source)
    {
        slot = inventorySlot;
        dragSource = source;
        slotContainer = source;
        slot.OnSlotChanged += Refresh;
        Refresh();
    }

    public void Refresh()
    {
        bool empty = slot.IsEmpty;
        icon.enabled = !empty;
        amountTextBg.enabled = !empty;
        amountText.text = empty ? "" : slot.amount.ToString();
        if (!empty) icon.sprite = slot.item.icon;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (slot.IsEmpty) return;
        dragSource.SetDraggedSlot(slot);
        DragItemUI.Instance.BeginDrag(slot, dragSource);
    }

    public void OnDrag(PointerEventData eventData) { }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        DragItemUI.Instance.EndDrag();
        dragSource.ClearDraggedSlot();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        HandleDrop(slot);
    }

    // Handles right-click WHILE dragging — place 1 item on this slot
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;

        var dragged = DragItemUI.Instance.DraggedSlot;
        var source = DragItemUI.Instance.DragSource;
        if (dragged == null) return;

        // Same item — just add 1
        if (slot.item == dragged.item && slot.item.stackable)
        {
            int space = slot.item.maxStack - slot.amount;
            if (space <= 0) return;
            slot.Add(1);
            dragged.Remove(1);
        }
        // Empty slot — place 1
        else if (slot.IsEmpty)
        {
            slot.Set(dragged.item, 1);
            dragged.Remove(1);
        }
        else return; // occupied by different item — do nothing

        // End drag if source is now empty
        if (dragged.IsEmpty)
        {
            DragItemUI.Instance.EndDrag();
            source?.ClearDraggedSlot();
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        // Right click — split in half (only when NOT dragging)
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (DragItemUI.Instance.DraggedSlot != null) return; // dragging handled by OnPointerDown
            if (slot.IsEmpty || slot.amount <= 1) return;

            InventorySlot emptySlot = slotContainer.FindNextEmptySlot();
            if (emptySlot == null)
            {
                Debug.Log("No empty slot to split into!");
                return;
            }

            int splitAmount = slot.amount / 2;
            emptySlot.Set(slot.item, splitAmount);
            slot.Remove(splitAmount);
            return;
        }

        // Left double click — collect matching items
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Time.time - lastClickTime <= DoubleClickThreshold)
                slotContainer.CollectMatchingItems(slot);

            lastClickTime = Time.time;
        }
    }

    protected void HandleDrop(InventorySlot target)
    {
        var dragged = DragItemUI.Instance.DraggedSlot;
        var source = DragItemUI.Instance.DragSource;

        if (dragged == null || dragged == target) return;

        if (target.item == dragged.item && target.item != null && target.item.stackable)
        {
            int space = target.item.maxStack - target.amount;
            int moveAmount = Mathf.Min(space, dragged.amount);
            if (moveAmount > 0)
            {
                target.Add(moveAmount);
                dragged.Remove(moveAmount);
            }
        }
        else
        {
            var tempItem = target.item;
            var tempAmount = target.amount;
            target.Set(dragged.item, dragged.amount);
            if (tempItem == null) dragged.Clear();
            else dragged.Set(tempItem, tempAmount);
        }

        DragItemUI.Instance.EndDrag();
        source?.ClearDraggedSlot();
    }
}
