using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyerIdleState : State
{
    public BuyerIdleState(Entity entity, FiniteStateMachine stateMachine, EntityData entityData, string animBoolName) : base(entity, stateMachine, entityData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
