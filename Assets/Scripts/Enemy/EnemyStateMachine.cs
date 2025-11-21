using UnityEngine;

public abstract class EnemyState
{
    protected EnemyController enemyController;
    protected GameObject owner;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Enemy enemy;

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
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}

public class EnemyIdleState : EnemyState
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
        timer += Time.deltaTime;

        if (enemyController.playerInRange)
        {
            enemyController.enemyStateMachine.ChangeState(new EnemyAttackState(owner));
            return;
        }

        if (timer >= enemyController.idleTimer)
        {
            enemyController.enemyStateMachine.ChangeState(new EnemyPatrolState(owner));
        }
    }
}

public class EnemyPatrolState : EnemyState
{
    public EnemyPatrolState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        animator.Play("Idle");

        if (enemyController.patrolPoints == null || enemyController.patrolPoints.Length < 2)
        {
            enemyController.enemyStateMachine.ChangeState(new EnemyIdleState(owner));
            return;
        }
    }

    public override void Update()
    {
        if (enemyController.playerInRange)
        {
            enemyController.enemyStateMachine.ChangeState(new EnemyAttackState(owner));
            return;
        }

        Transform targetPoint = enemyController.patrolPoints[enemyController.currentPatrolIndex];
        Vector2 currentPos = owner.transform.position;
        Vector2 targetPos = new Vector2(targetPoint.position.x, currentPos.y);

        rb.MovePosition(Vector2.MoveTowards(currentPos, targetPos, enemyController.velocity * Time.deltaTime));

        if (Mathf.Abs(currentPos.x - targetPos.x) < 0.05f)
        {
            // Flip sprite
            var sr = enemyController.GetComponent<SpriteRenderer>();
            sr.flipX = !sr.flipX;

            enemyController.currentPatrolIndex++;
            if (enemyController.currentPatrolIndex >= enemyController.patrolPoints.Length)
                enemyController.currentPatrolIndex = 0;

            enemyController.enemyStateMachine.ChangeState(new EnemyIdleState(owner));
        }
    }
}

public class EnemyAttackState : EnemyState
{
    private float attackTimer;

    public EnemyAttackState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        attackTimer = 0f;
        animator.Play("Attack");
    }

    public override void Update()
    {
        if (!enemyController.playerInRange)
        {
            enemyController.enemyStateMachine.ChangeState(new EnemyIdleState(owner));
            return;
        }

        attackTimer += Time.deltaTime;

        if (enemyController.enemyType == EnemyType.Mosca)
        {
            HandleMoscaAttack();
        }
        else if (enemyController.enemyType == EnemyType.Sapo)
        {
            HandleSapoAttack();
        }
    }

    private void HandleMoscaAttack()
    {
        if (enemyController.player == null || enemyController.projectilePrefab == null || enemyController.firePoint == null)
            return;

        // Shoot periodically
        if (attackTimer >= enemyController.attackCooldown)
        {
            attackTimer = 0f;

            // Compute direction to player
            Vector2 direction = (enemyController.player.position - enemyController.firePoint.position).normalized;

            // Flip sprite depending on player's position
            var sr = enemyController.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                bool playerIsOnRight = enemyController.player.position.x > owner.transform.position.x;
                sr.flipX = !playerIsOnRight; // assuming facing right by default
            }

            // Rotate firePoint to aim at the player (optional)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            enemyController.firePoint.rotation = Quaternion.Euler(0, 0, angle);

            // Instantiate projectile
            GameObject projectile = Object.Instantiate(
                enemyController.projectilePrefab,
                enemyController.firePoint.position,
                Quaternion.identity
            );

            var proj = projectile.GetComponent<EnemyProjectile>();
            if (proj != null)
                proj.SetDirection(direction);

            // Play attack animation again
            animator.Play("Attack");
        }
    }


    private void HandleSapoAttack()
    {
        //TODO
    }
}


public class EnemyDamagedState : EnemyState
{
    public EnemyDamagedState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        animator.Play("Damage");
    }

    public override void Update()
    {
        // Optional: return to idle after damaged anim
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            enemyController.enemyStateMachine.ChangeState(new EnemyIdleState(owner));
        }
    }
}

public class EnemyDeadState : EnemyState
{
    public EnemyDeadState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        animator.Play("Death");
        Object.Destroy(owner, 1f);
    }
}

