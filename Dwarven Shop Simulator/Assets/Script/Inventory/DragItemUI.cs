using UnityEngine;
using UnityEngine.UI;

public class DragItemUI : MonoBehaviour
{
    public static DragItemUI Instance;

    public Image icon;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(Sprite sprite)
    {
        icon.sprite = sprite;
        icon.enabled = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        icon.enabled = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}