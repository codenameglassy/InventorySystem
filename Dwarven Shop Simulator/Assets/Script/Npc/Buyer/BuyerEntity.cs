using UnityEngine;
using UnityEngine.AI;

public class BuyerEntity : Entity
{
    [Header("Buyer Settings")]
    public Transform spawnPoint;
    public Transform wanderCenter;

    public BuyerData BuyerData => entityData as BuyerData;
    public NavMeshAgent Agent { get; private set; }
    public SaleItem TargetItem { get; set; }

    // States
    public BuyerSpawnState spawnState;
    public BuyerIdleState idleState;
    public BuyerMoveToItemState moveToItemState;
    public BuyerInspectState inspectState;
    public BuyerBuyState buyState;

    protected override void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    protected override void Start()
    {
        stateMachine = new FiniteStateMachine();

        spawnState = new BuyerSpawnState(this, stateMachine, BuyerData, "move");
        idleState = new BuyerIdleState(this, stateMachine, BuyerData, "idle");
        moveToItemState = new BuyerMoveToItemState(this, stateMachine, BuyerData, "move");
        inspectState = new BuyerInspectState(this, stateMachine, BuyerData, "inspect");
        buyState = new BuyerBuyState(this, stateMachine, BuyerData, "idle");

        // Configure NavMesh agent
        Agent.speed = BuyerData.moveSpeed;
        Agent.stoppingDistance = BuyerData.stoppingDistance;

        stateMachine.Initialize(spawnState);
    }

    protected override void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }

    protected override void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    /// <summary>
    /// Finds highest priority item available in registry within budget.
    /// </summary>
    public SaleItem FindPriorityItem()
    {
        foreach (var priority in BuyerData.itemPriorities)
        {
            foreach (var saleItem in SaleItemRegistry.Instance.ItemsForSale)
            {
                if (saleItem.itemData == priority && saleItem.price <= BuyerData.budget)
                    return saleItem;
            }
        }
        return null;
    }

    public bool HasReachedDestination()
    {
        return !Agent.pathPending
            && Agent.remainingDistance <= Agent.stoppingDistance;
    }
}