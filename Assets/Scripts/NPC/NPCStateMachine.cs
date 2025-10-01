using UnityEngine;

public abstract class NPCState
{
    protected NPCController npcController;
    protected GameObject owner;
    protected Rigidbody2D rb;
    protected Animator animator;

    public NPCState(GameObject owner)
    {
        this.owner = owner;
        npcController = owner.GetComponent<NPCController>();
        rb = owner.GetComponent<Rigidbody2D>();
        animator = owner.GetComponent<Animator>();
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}

public class NPCStateMachine
{
    public NPCState CurrentState { get; private set; }

    public void ChangeState(NPCState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }

        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }
}

public class NPCIdleState : NPCState
{
    private float timer;
    private bool idleTimerExpired;

    public NPCIdleState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("NPC-Entering Idle State");
        timer = 0f;
        idleTimerExpired = false;
        rb.linearVelocityX = 0; // Stop movement
        animator.Play("Idle");

    }

    public override void Update()
    {
        if (!idleTimerExpired)
        {
            timer += Time.deltaTime;
            if (timer >= npcController.idleTimer)
            {
                idleTimerExpired = true;
                npcController.npcStateMachine.ChangeState(new NPCPatrolState(owner));
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("NPC-Exiting Idle State");
    }
}

public class NPCPatrolState : NPCState
{
    public NPCPatrolState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("NPC-Entering Patrol State");
        animator.Play("Walk");
    }

    public override void Update()
    {
        if (npcController.patrolPoints.Length == 0) return;

        Transform targetPoint = npcController.patrolPoints[npcController.currentPatrolIndex];

        // Move towards target
        rb.MovePosition(Vector2.MoveTowards(owner.transform.position, targetPoint.position, npcController.velocity * Time.deltaTime));

        // Check if reached
        if (Vector2.Distance(owner.transform.position, targetPoint.position) < 0.1f)
        {
            // Go to next point
            npcController.gameObject.GetComponent<SpriteRenderer>().flipX = !npcController.gameObject.GetComponent<SpriteRenderer>().flipX; // Flip sprite direction
            npcController.currentPatrolIndex++;
            if (npcController.currentPatrolIndex >= npcController.patrolPoints.Length)
            {
                npcController.currentPatrolIndex = 0;
            }

            // Switch to idle after reaching a point
            npcController.npcStateMachine.ChangeState(new NPCIdleState(owner));
        }
    }

    public override void Exit()
    {
        Debug.Log("NPC-Exiting Patrol State");
    }
}

public class NPCTalkingState : NPCState //TODO: Implement talking logic
{
    public NPCTalkingState(GameObject owner) : base(owner) { }
    public override void Enter()
    {
        Debug.Log("NPC-Entering Talking State");
        rb.linearVelocityX = 0; // Stop movement
        animator.Play("Idle");
    }
    public override void Update()
    {
        // Logic for talking, e.g., waiting for player input to finish conversation
    }
    public override void Exit()
    {
        Debug.Log("NPC-Exiting Talking State");
    }
}
