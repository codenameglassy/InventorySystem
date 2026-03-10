using System.Collections.Generic;
using UnityEngine;

public class SlotRegistry : MonoBehaviour
{
    public static SlotRegistry Instance { get; private set; }

    private readonly List<BaseSlotUI> registeredSlots = new List<BaseSlotUI>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Register(BaseSlotUI slot)
    {
        if (!registeredSlots.Contains(slot))
            registeredSlots.Add(slot);
    }

    public void Unregister(BaseSlotUI slot)
    {
        registeredSlots.Remove(slot);
    }

    /// <summary>
    /// Returns the closest slot UI to the given screen position within snapRadius.
    /// Returns null if none found within radius.
    /// </summary>
    public BaseSlotUI FindClosestSlot(Vector2 screenPosition, float snapRadius)
    {
        BaseSlotUI closest = null;
        float closestDistance = snapRadius;

        foreach (var slot in registeredSlots)
        {
            if (slot == null) continue;

            Vector2 slotScreenPos = RectTransformUtility.WorldToScreenPoint(
                null, slot.transform.position);

            float distance = Vector2.Distance(screenPosition, slotScreenPos);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = slot;
            }
        }

        return closest;
    }
}