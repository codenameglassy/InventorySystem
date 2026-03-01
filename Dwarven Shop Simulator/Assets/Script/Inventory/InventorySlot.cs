[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int amount;

    public bool IsEmpty => item == null;

    public void Set(ItemData item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public void Clear()
    {
        item = null;
        amount = 0;
    }
}