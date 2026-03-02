using UnityEngine;
using UnityEngine.UI;

public class DragItemUI : MonoBehaviour
{
    public static DragItemUI Instance;

    [SerializeField] private Image icon;
    [SerializeField] private CanvasGroup canvasGroup;

    public InventorySlot DraggedSlot { get; private set; }
    public IDragSource DragSource { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        EndDrag(); // ← was Hide()
    }

    private void Update()
    {
        if (icon.enabled)
            transform.position = Input.mousePosition;
    }

    public void BeginDrag(InventorySlot slot, IDragSource source)
    {
        if (slot == null || slot.IsEmpty) return;

        DraggedSlot = slot;
        DragSource = source;

        icon.sprite = slot.item.icon;
        icon.enabled = true;
        transform.position = Input.mousePosition;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 1f;
        gameObject.SetActive(true);
    }

    public void HideVisual()
    {
        icon.sprite = null;
        icon.enabled = false;
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    // Clears drag state — called after drop is resolved
    public void ClearState()
    {
        DraggedSlot = null;
        DragSource = null;
    }

    // Full cleanup — keeps convenience method for other uses
    public void EndDrag()
    {
        HideVisual();
        ClearState();
    }
}