using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    //Finite State Machine
    public FiniteStateMachine stateMachine;
    public EntityData entityData;
    public Animator anim;

    protected abstract void Awake();
    protected abstract void Start();
    protected abstract void Update();
    protected abstract void FixedUpdate();
}
