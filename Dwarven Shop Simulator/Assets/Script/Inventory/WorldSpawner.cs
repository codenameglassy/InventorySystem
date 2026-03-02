using System.Collections.Generic;
using UnityEngine;

public class WorldSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;         // ← assign one per store slot
    public StoreInventory storeInventory;

    private Dictionary<InventorySlot, GameObject> spawnedObjects = new Dictionary<InventorySlot, GameObject>();

    private void Start()
    {
        for (int i = 0; i < storeInventory.slots.Count; i++)
            SubscribeToSlot(storeInventory.slots[i], i);
    }

    private void SubscribeToSlot(InventorySlot slot, int index)
    {
        if (index >= spawnPoints.Count)
        {
            Debug.LogWarning($"No spawn point assigned for slot index {index}");
            return;
        }

        ItemData previousItem = null;

        slot.OnSlotChanged += () =>
        {
            if (slot.IsEmpty)
            {
                DespawnObject(slot);
            }
            else if (slot.item != previousItem)
            {
                DespawnObject(slot);
                SpawnObject(slot, spawnPoints[index]);
            }

            previousItem = slot.item;
        };
    }

    private void SpawnObject(InventorySlot slot, Transform point)
    {
        if (slot.item.worldPrefab == null) return;

        var obj = Instantiate(slot.item.worldPrefab, point.position, point.rotation);
        spawnedObjects[slot] = obj;
    }

    private void DespawnObject(InventorySlot slot)
    {
        if (!spawnedObjects.TryGetValue(slot, out var obj)) return;

        Destroy(obj);
        spawnedObjects.Remove(slot);
    }
}