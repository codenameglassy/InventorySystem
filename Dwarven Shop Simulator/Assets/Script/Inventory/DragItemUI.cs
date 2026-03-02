using UnityEngine;
using UnityEngine.UI;

public class DragItemUI : MonoBehaviour
{
    public static DragItemUI Instance;

    [SerializeField] private Image icon;
    [SerializeField] private CanvasGroup canvasGroup;

    // Global drag state accessible by any slot
    public InventorySlot DraggedSlot { get; private set; }
    public IDragSource DragSource { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        Hide();
    }

    private void Update()
    {
        if (icon.enabled)
            transform.position = Input.mousePosition;
    }

    public void Show(Sprite sprite, InventorySlot slot, IDragSource source)
    {
        icon.sprite = sprite;
        icon.enabled = true;
        transform.position = Input.mousePosition;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 1f;
        gameObject.SetActive(true);

        DraggedSlot = slot;
        DragSource = source;
    }

    public void Hide()
    {
        icon.sprite = null;
        icon.enabled = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);

        DraggedSlot = null;
        DragSource = null;
    }
}