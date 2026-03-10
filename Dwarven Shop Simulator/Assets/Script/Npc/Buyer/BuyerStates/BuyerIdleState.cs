using UnityEngine;

public class BuyerIdleState : State
{
    private BuyerEntity buyer;
    private BuyerData buyerData;
    private float wanderTimer;
    private const float WanderInterval = 3f;

    public BuyerIdleState(Entity entity, FiniteStateMachine stateMachine,
        EntityData entityData, string animBoolName)
        : base(entity, stateMachine, entityData, animBoolName)
    {
        buyer = entity as BuyerEntity;
        buyerData = entityData as BuyerData;
    }

    public override void Enter()
    {
        base.Enter();
        wanderTimer = WanderInterval;
        SetRandomDestination();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0)
        {
            wanderTimer = WanderInterval;
            SetRandomDestination();
        }

        // Check registry for priority items
        var item = buyer.FindPriorityItem();
        if (item != null)
        {
            buyer.TargetItem = item;
            stateMachine.ChangeState(buyer.moveToItemState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        buyer.Agent.ResetPath();
    }

    private void SetRandomDestination()
    {
        Vector3 randomPoint = buyer.wanderCenter.position
            + Random.insideUnitSphere * buyerData.idleWanderRadius;
        randomPoint.y = buyer.transform.position.y;

        if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out var hit, 2f,
            UnityEngine.AI.NavMesh.AllAreas))
        {
            buyer.Agent.SetDestination(hit.position);
        }
    }
}