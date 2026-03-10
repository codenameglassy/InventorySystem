public class BuyerMoveToItemState : State
{
    private BuyerEntity buyer;

    public BuyerMoveToItemState(Entity entity, FiniteStateMachine stateMachine,
        EntityData entityData, string animBoolName)
        : base(entity, stateMachine, entityData, animBoolName)
    {
        buyer = entity as BuyerEntity;
    }

    public override void Enter()
    {
        base.Enter();

        if (buyer.TargetItem == null)
        {
            stateMachine.ChangeState(buyer.idleState);
            return;
        }

        buyer.Agent.SetDestination(buyer.TargetItem.worldTransform.position);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Item was bought by someone else
        if (buyer.TargetItem == null ||
            !SaleItemRegistry.Instance.Contains(buyer.TargetItem))
        {
            buyer.TargetItem = null;
            stateMachine.ChangeState(buyer.idleState);
            return;
        }

        if (buyer.HasReachedDestination())
            stateMachine.ChangeState(buyer.inspectState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}