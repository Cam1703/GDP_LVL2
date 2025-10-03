using UnityEngine;

public abstract class NPCState
{
    protected NPCController npcController;
    protected GameObject owner;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected bool playerInRange => owner.GetComponentInChildren<DialogueTrigger>().playerInRange;

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
            CurrentState.Exit();

        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        if (CurrentState != null)
            CurrentState.Update();
    }
}

public class NPCIdleState : NPCState
{
    private float timer;

    public NPCIdleState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("NPC-Entering Idle State");
        timer = 0f;
        rb.linearVelocity = Vector2.zero;
        animator.Play("Idle");
    }

    public override void Update()
    {
        // Priority: Dialogue first
        if (DialogueManager.Instance.IsDialoguePlaying && playerInRange)
        {
            npcController.npcStateMachine.ChangeState(new NPCTalkingState(owner));
            return;
        }

        // Patrol after idle timer
        timer += Time.deltaTime;
        if (timer >= npcController.idleTimer)
        {
            npcController.npcStateMachine.ChangeState(new NPCPatrolState(owner));
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
        // Priority: Dialogue first
        if (DialogueManager.Instance.IsDialoguePlaying && playerInRange)
        {
            npcController.npcStateMachine.ChangeState(new NPCTalkingState(owner));
            return;
        }

        if (npcController.patrolPoints.Length == 0) return;

        Transform targetPoint = npcController.patrolPoints[npcController.currentPatrolIndex];

        // Move towards target
        rb.MovePosition(Vector2.MoveTowards(owner.transform.position, targetPoint.position, npcController.velocity * Time.deltaTime));

        // Check if reached
        if (Vector2.Distance(owner.transform.position, targetPoint.position) < 0.1f)
        {
            npcController.gameObject.GetComponent<SpriteRenderer>().flipX = !npcController.gameObject.GetComponent<SpriteRenderer>().flipX;

            npcController.currentPatrolIndex++;
            if (npcController.currentPatrolIndex >= npcController.patrolPoints.Length)
                npcController.currentPatrolIndex = 0;

            npcController.npcStateMachine.ChangeState(new NPCIdleState(owner));
        }
    }

    public override void Exit()
    {
        Debug.Log("NPC-Exiting Patrol State");
    }
}

public class NPCTalkingState : NPCState
{
    private RigidbodyConstraints2D originalConstraints;

    public NPCTalkingState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("NPC-Entering Talking State");
        originalConstraints = rb.constraints;
        rb.constraints = RigidbodyConstraints2D.FreezePosition; // freeze movement
        animator.Play("Idle");
    }

    public override void Update()
    {
        if (!DialogueManager.Instance.IsDialoguePlaying)
        {
            npcController.npcStateMachine.ChangeState(new NPCIdleState(owner));
        }
    }

    public override void Exit()
    {
        Debug.Log("NPC-Exiting Talking State");
        rb.constraints = originalConstraints; // restore previous constraints
    }
}
