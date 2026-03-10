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
        slotContainer = source;
        slot.OnSlotChanged += Refresh;
        Refresh();
    }

    private void OnEnable()
    {
        // Wait for SlotRegistry to be available
        if (SlotRegistry.Instance != null)
            SlotRegistry.Instance.Register(this);
    }

    private void OnDisable()
    {
        if (SlotRegistry.Instance != null)
            SlotRegistry.Instance.Unregister(this);
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
        // If snapping to a slot, drop there
        var snapTarget = DragItemUI.Instance.SnapTarget;
        if (snapTarget != null && snapTarget != this)
            HandleDrop(snapTarget.GetSlot());

        DragItemUI.Instance.HideVisual();
        dragSource.ClearDraggedSlot();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        // Only handle if not already handled by snap in OnEndDrag
        if (DragItemUI.Instance.SnapTarget != null) return;
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

    // Expose slot for snap target access
    public InventorySlot GetSlot() => slot;

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

        DragItemUI.Instance.ClearState();
        source?.ClearDraggedSlot();
    }
}
