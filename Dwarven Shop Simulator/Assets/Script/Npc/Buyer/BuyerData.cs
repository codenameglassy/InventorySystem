using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuyerData", menuName = "EntityData/Buyer")]
public class BuyerData : EntityData
{
    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float stoppingDistance = 1.5f;

    [Header("Behaviour")]
    public float idleWanderRadius = 5f;
    public float inspectDuration = 2f;
    public int budget = 100;

    [Header("Item Priorities (top = highest priority)")]
    public List<ItemData> itemPriorities = new List<ItemData>();
}