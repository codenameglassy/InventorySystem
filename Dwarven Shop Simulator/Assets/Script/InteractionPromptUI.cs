using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    public static InteractionPromptUI Instance { get; private set; }

    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TMP_Text promptText;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        Hide();
    }

    public void Show(string message)
    {
        promptText.text = message;
        promptPanel.SetActive(true);
    }

    public void Hide()
    {
        promptPanel.SetActive(false);
    }
}