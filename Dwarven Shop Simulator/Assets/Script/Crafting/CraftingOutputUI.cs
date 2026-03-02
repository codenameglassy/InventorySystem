using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingOutputUI : MonoBehaviour
{
    [SerializeField] private Image outputIcon;
    [SerializeField] private TMP_Text outputAmountText;
    [SerializeField] private Button craftButton;

    public void ShowOutput(CraftingRecipe recipe)
    {
        outputIcon.enabled = true;
        outputIcon.sprite = recipe.outputItem.icon;
        outputAmountText.text = recipe.outputAmount.ToString();
        craftButton.interactable = true;
    }

    public void ClearOutput()
    {
        outputIcon.enabled = false;
        outputAmountText.text = "";
        craftButton.interactable = false;
    }

    public void SetCraftAction(UnityEngine.Events.UnityAction action)
    {
        craftButton.onClick.RemoveAllListeners();
        craftButton.onClick.AddListener(action);
    }
}