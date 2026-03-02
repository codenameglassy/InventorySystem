using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public abstract class BaseSlotUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
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
        slotContainer = source;     // same object implements both interfaces
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
        // Only hide the visual — don't clear state yet
        // OnDrop fires after this and still needs DraggedSlot + DragSource
        DragItemUI.Instance.HideVisual();
        dragSource.ClearDraggedSlot();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        HandleDrop(slot);
    }

  
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        // Right click — split in half
        if (eventData.button == PointerEventData.InputButton.Right)
        {
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

        // Clear state here after drop is resolved
        DragItemUI.Instance.ClearState();
        source?.ClearDraggedSlot();
    }
}