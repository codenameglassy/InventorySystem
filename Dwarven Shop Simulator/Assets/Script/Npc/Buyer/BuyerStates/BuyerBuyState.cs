using UnityEngine;

public class BuyerBuyState : State
{
    private BuyerEntity buyer;

    public BuyerBuyState(Entity entity, FiniteStateMachine stateMachine,
        EntityData entityData, string animBoolName)
        : base(entity, stateMachine, entityData, animBoolName)
    {
        buyer = entity as BuyerEntity;
    }

    public override void Enter()
    {
        base.Enter();

        if (buyer.TargetItem == null) return;

        // Deduct budget
        buyer.BuyerData.budget -= buyer.TargetItem.price;

        // Remove item from store slot
        buyer.TargetItem.sourceSlot.Clear();

        // Unregister from sale registry
        SaleItemRegistry.Instance.Unregister(buyer.TargetItem);

        Debug.Log($"Buyer purchased {buyer.TargetItem.itemData.itemName} " +
                  $"for {buyer.TargetItem.price}. " +
                  $"Remaining budget: {buyer.BuyerData.budget}");

        buyer.TargetItem = null;

        // Walk back to spawn point and leave
        buyer.Agent.SetDestination(buyer.spawnPoint.position);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Reached spawn point — destroy buyer
        if (buyer.HasReachedDestination())
            UnityEngine.Object.Destroy(buyer.gameObject);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
