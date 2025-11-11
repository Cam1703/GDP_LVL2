using UnityEngine;

public abstract class EnemyState
{
    protected EnemyController enemyController;
    protected GameObject owner;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Enemy enemy;
    protected bool playerInRange;

    public EnemyState(GameObject owner)
    {
        this.owner = owner;
        enemyController = owner.GetComponent<EnemyController>();
        rb = owner.GetComponent<Rigidbody2D>();
        animator = owner.GetComponent<Animator>();
        enemy = owner.GetComponent<Enemy>();
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
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
        if (CurrentState != null)
            CurrentState.Update();
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
            // 
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