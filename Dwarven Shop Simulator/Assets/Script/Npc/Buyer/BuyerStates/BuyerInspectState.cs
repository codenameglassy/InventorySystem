using UnityEngine;

public class BuyerInspectState : State
{
    private BuyerEntity buyer;
    private BuyerData buyerData;

    public BuyerInspectState(Entity entity, FiniteStateMachine stateMachine,
        EntityData entityData, string animBoolName)
        : base(entity, stateMachine, entityData, animBoolName)
    {
        buyer = entity as BuyerEntity;
        buyerData = entityData as BuyerData;
    }

    public override void Enter()
    {
        base.Enter();
        buyer.Agent.ResetPath();

        // Face the item
        if (buyer.TargetItem != null)
        {
            Vector3 direction = buyer.TargetItem.worldTransform.position - buyer.transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
                buyer.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Item no longer available
        if (buyer.TargetItem == null ||
            !SaleItemRegistry.Instance.Contains(buyer.TargetItem))
        {
            buyer.TargetItem = null;
            stateMachine.ChangeState(buyer.idleState);
            return;
        }

        // Wait for inspect duration then buy
        if (Time.time - startTime >= buyerData.inspectDuration)
            stateMachine.ChangeState(buyer.buyState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}