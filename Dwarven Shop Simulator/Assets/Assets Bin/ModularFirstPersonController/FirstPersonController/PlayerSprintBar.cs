using UnityEngine;
using UnityEngine.UI;

public class PlayerSprintBar : MonoBehaviour
{
    [SerializeField] private bool useSprintBar = true;
    [SerializeField] private bool hideBarWhenFull = true;
    [SerializeField] private Image sprintBarBG;
    [SerializeField] private Image sprintBar;
    [SerializeField] private float barWidthPercent = 0.3f;
    [SerializeField] private float barHeightPercent = 0.015f;

    private PlayerState state;
    private PlayerMovement playerMovement;
    private CanvasGroup canvasGroup;

    private const float FadeInSpeed = 5f;
    private const float FadeOutSpeed = 3f;

    public void Initialize(PlayerState playerState)
    {
        state = playerState;
        playerMovement = GetComponent<PlayerMovement>();
        canvasGroup = GetComponentInChildren<CanvasGroup>();

        if (!useSprintBar)
        {
            sprintBarBG.gameObject.SetActive(false);
            sprintBar.gameObject.SetActive(false);
            return;
        }

        sprintBarBG.gameObject.SetActive(true);
        sprintBar.gameObject.SetActive(true);

        float width = Screen.width * barWidthPercent;
        float height = Screen.height * barHeightPercent;

        sprintBarBG.rectTransform.sizeDelta = new Vector2(width, height);
        sprintBar.rectTransform.sizeDelta = new Vector2(width - 2, height - 2);

        if (hideBarWhenFull) canvasGroup.alpha = 0;
    }

    private void Update()
    {
        if (!useSprintBar) return;

        UpdateBarFill();
        UpdateBarVisibility();
    }

    private void UpdateBarFill()
    {
        float percent = state.SprintRemaining / playerMovement.SprintDuration;
        sprintBar.transform.localScale = new Vector3(percent, 1f, 1f);
    }

    private void UpdateBarVisibility()
    {
        if (!hideBarWhenFull) return;

        if (state.IsSprinting)
            canvasGroup.alpha += FadeInSpeed * Time.deltaTime;
        else if (state.SprintRemaining >= playerMovement.SprintDuration)
            canvasGroup.alpha -= FadeOutSpeed * Time.deltaTime;

        canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha);
    }
}