using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;
    public EntityData entityData;
    public Animator anim;

    protected virtual void Awake() { }
    protected virtual void Start() { }
    protected virtual void Update() { }
    protected virtual void FixedUpdate() { }
}