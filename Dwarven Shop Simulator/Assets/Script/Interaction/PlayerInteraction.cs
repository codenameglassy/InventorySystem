using UnityEngine;

public class PlayerInteraction : MonoBehaviour, IGameStateObserver
{
    [SerializeField] private float interactionRadius = 3f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private LayerMask interactableLayer;

    private IInteractable currentInteractable;
    private bool canInteract = true;

    private void Start()
    {
        GameStateManager.Instance.Register(this);
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.Unregister(this);
    }

    private void Update()
    {
        if (!canInteract) return;

        FindClosestInteractable();

        if (currentInteractable != null && Input.GetKeyDown(interactKey))
            currentInteractable.Interact();
    }

    private void FindClosestInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius, interactableLayer);

        IInteractable closest = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent<IInteractable>(out var interactable)) continue;

            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = interactable;
            }
        }

        // Changed
        if (closest != currentInteractable)
        {
            currentInteractable = closest;

            if (currentInteractable != null)
                InteractionPromptUI.Instance.Show(currentInteractable.PromptMessage);
            else
                InteractionPromptUI.Instance.Hide();
        }
    }

    // Disable interaction while inventory is open
    public void OnStateChanged(GameState newState)
    {
        canInteract = newState == GameState.Normal;

        if (!canInteract)
        {
            currentInteractable = null;
            InteractionPromptUI.Instance.Hide();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}