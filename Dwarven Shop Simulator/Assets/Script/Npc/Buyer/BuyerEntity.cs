using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyerEntity : Entity
{
    //Finiste state machine
    public BuyerIdleState idleState;



    protected override void Awake()
    {
      
    }

    protected override void Start()
    {
        stateMachine = new FiniteStateMachine();
        idleState = new BuyerIdleState(this, stateMachine, entityData, "idle");
    }
   
    protected override void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }

    protected override void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

}
