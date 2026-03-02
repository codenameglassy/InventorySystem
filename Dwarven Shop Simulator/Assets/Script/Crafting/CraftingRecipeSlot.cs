using System;
using UnityEngine;

[Serializable]
public class CraftingRecipeSlot
{
    public ItemData item;       // null = empty slot
    public int amount = 1;
}