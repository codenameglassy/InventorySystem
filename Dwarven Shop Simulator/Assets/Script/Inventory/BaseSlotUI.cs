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
        DragItemUI.Instance.Show(slot.item.icon);
    }

    public void OnDrag(PointerEventData eventData) { }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        DragItemUI.Instance.Hide();
        dragSource.ClearDraggedSlot();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        dragSource.DropOnSlot(slot);
    }
}