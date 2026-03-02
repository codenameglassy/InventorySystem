using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 3f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private LayerMask interactableLayer;

    private IInteractable currentInteractable;
    private StorageInteractable activeStorage;

    private void Update()
    {
        FindClosestInteractable();

        if (Input.GetKeyDown(interactKey))
            HandleInteractInput();
    }

    private void FindClosestInteractable()
    {
        // Don't re-scan while storage is open
        if (activeStorage != null) return;

        Collider[] hits = Physics.OverlapSphere(
            transform.position, interactionRadius, interactableLayer);

        IInteractable closest = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent<IInteractable>(out var interactable)) continue;

            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance >= closestDistance) continue;

            closestDistance = distance;
            closest = interactable;
        }

        if (closest != currentInteractable)
        {
            currentInteractable = closest;

            if (currentInteractable != null)
                InteractionPromptUI.Instance.Show(currentInteractable.PromptMessage);
            else
                InteractionPromptUI.Instance.Hide();
        }
    }

    private void HandleInteractInput()
    {
        // Close active storage with E
        if (activeStorage != null)
        {
            activeStorage.Interact();
            activeStorage = null;
            InteractionPromptUI.Instance.Hide();
            return;
        }

        // Open nearby interactable with E
        if (currentInteractable == null) return;

        currentInteractable.Interact();

        // Track if we opened a storage
        if (currentInteractable is StorageInteractable storage)
        {
            activeStorage = storage;
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