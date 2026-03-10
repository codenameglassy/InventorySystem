using UnityEngine;

public class SaleItem
{
    public ItemData itemData;
    public int price;
    public Transform worldTransform; // where the prefab spawned in the world
    public InventorySlot sourceSlot; // so we can remove it from store when bought

    public SaleItem(ItemData itemData, int price, Transform worldTransform, InventorySlot sourceSlot)
    {
        this.itemData = itemData;
        this.price = price;
        this.worldTransform = worldTransform;
        this.sourceSlot = sourceSlot;
    }
}