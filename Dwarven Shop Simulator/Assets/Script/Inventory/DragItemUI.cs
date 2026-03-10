using UnityEngine;
using UnityEngine.UI;

public class DragItemUI : MonoBehaviour
{
    public static DragItemUI Instance { get; private set; }

    [SerializeField] private Image icon;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Snap Settings")]
    [SerializeField] private float snapRadius = 60f;

    public InventorySlot DraggedSlot { get; private set; }
    public IDragSource DragSource { get; private set; }
    public BaseSlotUI SnapTarget { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        EndDrag();
    }

    private void Update()
    {
        if (!icon.enabled) return;

        Vector2 mousePos = Input.mousePosition;

        // Find closest slot within snap radius
        SnapTarget = SlotRegistry.Instance.FindClosestSlot(mousePos, snapRadius);

        // Snap to slot or follow mouse
        if (SnapTarget != null)
            transform.position = SnapTarget.transform.position;
        else
            transform.position = mousePos;
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

    public void ClearState()
    {
        DraggedSlot = null;
        DragSource = null;
        SnapTarget = null;
    }

    public void EndDrag()
    {
        HideVisual();
        ClearState();
    }
}