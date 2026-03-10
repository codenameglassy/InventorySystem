using System.Collections.Generic;
using UnityEngine;

public class WorldSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public StoreInventory storeInventory;

    [Header("Sale Settings")]
    [SerializeField] private bool isSaleStore = false;

    private Dictionary<InventorySlot, GameObject> spawnedObjects
        = new Dictionary<InventorySlot, GameObject>();
    private Dictionary<InventorySlot, SaleItem> saleItems
        = new Dictionary<InventorySlot, SaleItem>();

    private void Start()
    {
        for (int i = 0; i < storeInventory.slots.Count; i++)
            SubscribeToSlot(storeInventory.slots[i], i);
    }

    private void SubscribeToSlot(InventorySlot slot, int index)
    {
        if (index >= spawnPoints.Count)
        {
            Debug.LogWarning($"No spawn point for slot {index}");
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

        // Register with sale registry if this is a sale store
        if (isSaleStore)
        {
            var saleItem = new SaleItem(slot.item, slot.item.price, point, slot);
            saleItems[slot] = saleItem;
            SaleItemRegistry.Instance.Register(saleItem);
        }
    }

    private void DespawnObject(InventorySlot slot)
    {
        if (spawnedObjects.TryGetValue(slot, out var obj))
        {
            Destroy(obj);
            spawnedObjects.Remove(slot);
        }

        // Unregister from sale registry
        if (isSaleStore && saleItems.TryGetValue(slot, out var saleItem))
        {
            SaleItemRegistry.Instance.Unregister(saleItem);
            saleItems.Remove(slot);
        }
    }
}
