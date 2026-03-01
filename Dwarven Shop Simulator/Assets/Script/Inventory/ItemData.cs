using UnityEngine;

public enum ItemType
{
    Material,
    Weapon,
    Armor,
    Consumable
}

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    [Header("Stack Settings")]
    public bool stackable = true;
    public int maxStack = 99;

    [Header("World Settings")]
    public GameObject worldPrefab;
}