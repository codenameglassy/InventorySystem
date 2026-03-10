using UnityEngine;

public class BuyerSpawnState : State
{
    private BuyerEntity buyer;

    public BuyerSpawnState(Entity entity, FiniteStateMachine stateMachine,
        EntityData entityData, string animBoolName)
        : base(entity, stateMachine, entityData, animBoolName)
    {
        buyer = entity as BuyerEntity;
    }

    public override void Enter()
    {
        base.Enter();
        buyer.Agent.SetDestination(buyer.wanderCenter.position);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!buyer.HasReachedDestination()) return;

        // Arrived — check for items or go idle
        var item = buyer.FindPriorityItem();
        if (item != null)
        {
            buyer.TargetItem = item;
            stateMachine.ChangeState(buyer.moveToItemState);
        }
        else
        {
            stateMachine.ChangeState(buyer.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}