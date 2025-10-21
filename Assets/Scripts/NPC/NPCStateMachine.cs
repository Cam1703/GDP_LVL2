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
        //Debug.Log("NPC-Entering Idle State");
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
        //Debug.Log("NPC-Exiting Idle State");
    }
}

public class NPCPatrolState : NPCState
{
    public NPCPatrolState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        //Debug.Log("NPC-Entering Patrol State");
        animator.Play("Walk");

        // If not enough patrol points, go back to idle
        if (npcController.patrolPoints == null || npcController.patrolPoints.Length < 2)
        {
            npcController.npcStateMachine.ChangeState(new NPCIdleState(owner));
        }
    }

    public override void Update()
    {
        // Priority: Dialogue first
        if (DialogueManager.Instance.IsDialoguePlaying && playerInRange)
        {
            npcController.npcStateMachine.ChangeState(new NPCTalkingState(owner));
            return;
        }

        if (npcController.patrolPoints == null || npcController.patrolPoints.Length < 2)
        {
            npcController.npcStateMachine.ChangeState(new NPCIdleState(owner));
            return;
        }

        Transform targetPoint = npcController.patrolPoints[npcController.currentPatrolIndex];

        // Move only along the X axis
        Vector2 currentPosition = owner.transform.position;
        Vector2 targetPosition = new Vector2(targetPoint.position.x, currentPosition.y);

        rb.MovePosition(Vector2.MoveTowards(currentPosition, targetPosition, npcController.velocity * Time.deltaTime));

        // Check if reached horizontally
        if (Mathf.Abs(currentPosition.x - targetPoint.position.x) < 0.1f)
        {
            // Flip sprite horizontally
            SpriteRenderer sr = npcController.gameObject.GetComponent<SpriteRenderer>();
            sr.flipX = !sr.flipX;

            npcController.currentPatrolIndex++;
            if (npcController.currentPatrolIndex >= npcController.patrolPoints.Length)
                npcController.currentPatrolIndex = 0;

            npcController.npcStateMachine.ChangeState(new NPCIdleState(owner));
        }
    }

    public override void Exit()
    {
        //Debug.Log("NPC-Exiting Patrol State");
    }
}

public class NPCTalkingState : NPCState
{
    private RigidbodyConstraints2D originalConstraints;

    public NPCTalkingState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        //Debug.Log("NPC-Entering Talking State");
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
        //Debug.Log("NPC-Exiting Talking State");
        rb.constraints = originalConstraints; // restore previous constraints
    }
}
