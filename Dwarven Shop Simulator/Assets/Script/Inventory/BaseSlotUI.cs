using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public abstract class BaseSlotUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("UI Elements")]
    [SerializeField] protected Image icon;
    [SerializeField] protected TMP_Text amountText;
    [SerializeField] protected Image amountTextBg;

    protected InventorySlot slot;
    protected IDragSource dragSource;

    protected void BaseInitialize(InventorySlot inventorySlot, IDragSource source)
    {
        slot = inventorySlot;
        dragSource = source;
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

    // Shared stack/swap logic — used by all slot types
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