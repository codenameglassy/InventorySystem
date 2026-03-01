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

    private InventorySlot storeSlot;
    private InventoryUI playerInventoryUI;

    public void Initialize(InventorySlot slot, InventoryUI playerUI)
    {
        storeSlot = slot;
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (storeSlot.IsEmpty) return;

        // Treat dragged item as coming from store
        playerInventoryUI.SetDraggedSlot(storeSlot);
        DragItemUI.Instance.Show(storeSlot.item.icon);
    }

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragItemUI.Instance.Hide();
        playerInventoryUI.ClearDraggedSlot();
    }

    public void OnDrop(PointerEventData eventData)
    {
        playerInventoryUI.DropOnSlot(storeSlot);
    }
}