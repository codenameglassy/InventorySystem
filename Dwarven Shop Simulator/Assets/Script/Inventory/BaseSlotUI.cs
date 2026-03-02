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

        // Pass slot + source globally so ANY slot can access it on drop
        DragItemUI.Instance.Show(slot.item.icon, slot, dragSource);
    }

    public void OnDrag(PointerEventData eventData) { }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        DragItemUI.Instance.Hide();
        dragSource.ClearDraggedSlot();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        var draggedSlot = DragItemUI.Instance.DraggedSlot;
        var source = DragItemUI.Instance.DragSource;

        if (draggedSlot == null || draggedSlot == slot) return;

        // Stack if same item and stackable
        if (slot.item == draggedSlot.item && slot.item.stackable)
        {
            int space = slot.item.maxStack - slot.amount;
            int moveAmount = Mathf.Min(space, draggedSlot.amount);

            if (moveAmount > 0)
            {
                slot.Add(moveAmount);
                draggedSlot.Remove(moveAmount);
            }
        }
        else
        {
            // Swap
            var tempItem = slot.item;
            var tempAmount = slot.amount;

            slot.Set(draggedSlot.item, draggedSlot.amount);

            if (tempItem == null) draggedSlot.Clear();
            else draggedSlot.Set(tempItem, tempAmount);
        }

        DragItemUI.Instance.Hide();
        source?.ClearDraggedSlot();
    }
}