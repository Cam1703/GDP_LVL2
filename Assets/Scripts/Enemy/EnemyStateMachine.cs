using UnityEngine;

public abstract class EnemyState
{
    protected EnemyController enemyController;
    protected GameObject owner;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Enemy enemy;
    protected bool playerInRange;
    public CircleCollider2D range;


    public EnemyState(GameObject owner)
    {
        this.owner = owner;
        enemyController = owner.GetComponent<EnemyController>();
        rb = owner.GetComponent<Rigidbody2D>();
        animator = owner.GetComponent<Animator>();
        enemy = owner.GetComponent<Enemy>();
        range = owner.GetComponent<CircleCollider2D>();
    }

    public virtual void Enter() { }
    public virtual void Update() {
        if (playerInRange)
        {
            enemyController.enemyStateMachine.ChangeState(new EnemyAttackState(owner));
        }


    }
    public virtual void Exit() { }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}

public class EnemyStateMachine
{
    public EnemyState CurrentState { get; private set; }

    public void ChangeState(EnemyState newState)
    {
        if (CurrentState != null)
            CurrentState.Exit();

        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}


public class EnemyIdleState: EnemyState
{
    private float timer;

    public EnemyIdleState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        timer = 0f;
        rb.linearVelocity = Vector2.zero;
        animator.Play("Idle");
    }

    public override void Update()
    {
        // Patrol after idle timer
        timer += Time.deltaTime;
        if (timer >= enemyController.idleTimer)
        {
            enemyController.enemyStateMachine.ChangeState(new EnemyPatrolState(owner));
        }
    }

    public override void Exit()
    {
    }

}

public class EnemyPatrolState: EnemyState
{
    public EnemyPatrolState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        //Debug.Log("NPC-Entering Patrol State");
        animator.Play("Walk");

        // If not enough patrol points, go back to idle
        if (enemyController.patrolPoints == null || enemyController.patrolPoints.Length < 2)
        {
            enemyController.enemyStateMachine.ChangeState(new EnemyIdleState(owner));
        }
    }

    public override void Update()
    {

        Transform targetPoint = enemyController.patrolPoints[enemyController.currentPatrolIndex];

        // Move only along the X axis
        Vector2 currentPosition = owner.transform.position;
        Vector2 targetPosition = new Vector2(targetPoint.position.x, currentPosition.y);

        rb.MovePosition(Vector2.MoveTowards(currentPosition, targetPosition, enemyController.velocity * Time.deltaTime));

        // Check if reached horizontally
        if (Mathf.Abs(currentPosition.x - targetPoint.position.x) < 0.1f)
        {
            // Flip sprite horizontally
            SpriteRenderer sr = enemyController.gameObject.GetComponent<SpriteRenderer>();
            sr.flipX = !sr.flipX;

            enemyController.currentPatrolIndex++;
            if (enemyController.currentPatrolIndex >= enemyController.patrolPoints.Length)
                enemyController.currentPatrolIndex = 0;

            enemyController.enemyStateMachine.ChangeState(new EnemyIdleState(owner));
        }
    }

    public override void Exit()
    {

    }
}

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(GameObject owner) : base(owner) { }
    public override void Enter()
    {
        animator.Play("Attack");
    }
    public override void Update()
    {
        // Attack logic here
        if(enemyController.enemyType == EnemyType.Mosca)
        {
            // Mosca specific attack logic
            // Shoot projectile towards player

        }
        else if (enemyController.enemyType == EnemyType.Sapo)
        {
            // Sapo specific attack logic
            // Leap towards player
        }
    }
    public override void Exit()
    {

    }
}

public class EnemyDamagedState : EnemyState
{
    public EnemyDamagedState(GameObject owner) : base(owner) { }
    public override void Enter()
    {
        animator.Play("Damaged");
    }
    public override void Update()
    {
        // Damaged logic here

    }
    public override void Exit()
    {
    }
}

public class EnemyDeadState : EnemyState
{
    public EnemyDeadState(GameObject owner) : base(owner) { }
    public override void Enter()
    {
        animator.Play("Dead");
        Object.Destroy(owner, 1f); // Destroy after 1 second
    }
    public override void Update()
    {
        // Dead logic here
    }
    public override void Exit()
    {

    }
}