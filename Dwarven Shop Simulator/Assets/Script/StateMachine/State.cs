using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{
    protected FiniteStateMachine stateMachine;
    protected Entity entity;
    protected EntityData entityData;
    protected float startTime;
    protected string animBoolName;
    public State(Entity entity, FiniteStateMachine stateMachine, EntityData entityData, string animBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.entityData = entityData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
        entity.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        entity.anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

}
