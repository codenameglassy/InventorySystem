using System;
using System.Collections.Generic;
using UnityEngine;

public class SaleItemRegistry : MonoBehaviour
{
    public static SaleItemRegistry Instance { get; private set; }

    private readonly List<SaleItem> itemsForSale = new List<SaleItem>();
    public IReadOnlyList<SaleItem> ItemsForSale => itemsForSale;

    public event Action OnRegistryChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Register(SaleItem item)
    {
        if (!itemsForSale.Contains(item))
        {
            itemsForSale.Add(item);
            OnRegistryChanged?.Invoke();
            Debug.Log($"{item.itemData.itemName} added to sale registry.");
        }
    }

    public void Unregister(SaleItem item)
    {
        if (itemsForSale.Remove(item))
        {
            OnRegistryChanged?.Invoke();
            Debug.Log($"{item.itemData.itemName} removed from sale registry.");
        }
    }

    public bool HasItems() => itemsForSale.Count > 0;

    public bool Contains(SaleItem item) => itemsForSale.Contains(item);
}