using UnityEngine;
using UnityEngine.UI;

public class DragItemUI : MonoBehaviour
{
    public static DragItemUI Instance;

    [SerializeField] private Image icon;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        Instance = this;
        Hide(); // important
    }

    private void Update()
    {
        if (icon.enabled)
            transform.position = Input.mousePosition;
    }


    public void Show(Sprite sprite)
    {
        icon.sprite = sprite;
        icon.enabled = true;

        canvasGroup.blocksRaycasts = false; // important
        canvasGroup.alpha = 1f;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        icon.sprite = null;
        icon.enabled = false;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

}