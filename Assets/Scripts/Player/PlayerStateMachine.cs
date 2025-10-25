using UnityEngine;
using Unity.IO.LowLevel.Unsafe;
using static UnityEngine.RuleTile.TilingRuleOutput;

public abstract class State
{
    private const float distanceToCheck = 0.05f;
    protected GameObject owner;

    protected bool isGrounded;
    protected Rigidbody2D rb;
    protected PlayerController playerController;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    public State(GameObject owner)
    {
        this.owner = owner;
        this.rb = owner.GetComponent<Rigidbody2D>();
        this.playerController = owner.GetComponent<PlayerController>();
        this.animator = owner.GetComponent<Animator>();
        this.spriteRenderer = owner.GetComponent<SpriteRenderer>();
    }

    public virtual void Enter()
    {
        // Asegura que no rote en ningún momento
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public virtual void Update()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(owner.transform.position, owner.transform.position + Vector3.down * distanceToCheck);

        if(InputManager.attack)
        {
            playerController.playerStateMachine.ChangeState(new AttackState(owner));
        }
        if (Physics2D.Raycast(owner.transform.position, Vector2.down, distanceToCheck, LayerMask.GetMask("Water")))
        {
            isGrounded = true;

            if (DialogueManager.Instance.IsDialoguePlaying && !(this is TalkState))
            {
                playerController.playerStateMachine.ChangeState(new TalkState(owner));
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    public virtual void Exit() { }

    public virtual void UpdateMovementX()
    {
        if (InputManager.movement.x != 0f)
        {
            rb.linearVelocityX = Mathf.Sign(InputManager.movement.x) * playerController.velocity;

            //Voltea el sprite según la dirección
            spriteRenderer.flipX = InputManager.movement.x < 0f;
        }
        else
        {
            rb.linearVelocityX = 0f;
        }
    }
}

public class StateMachine
{
    public State CurrentState { get; private set; }

    public void ChangeState(State newState)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }

        CurrentState = newState;

        if (CurrentState != null)
        {
            CurrentState.Enter();
        }
    }

    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }
}

public class IdleState : State
{
    public IdleState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        base.Enter();
        animator.Play("Idle");
        Debug.Log("Entrando a Idle");
    }

    public override void Update()
    {
        base.Update();

        if (InputManager.movement.x != 0f && isGrounded)
        {
            playerController.playerStateMachine.ChangeState(new WalkState(owner));
        }
        if (InputManager.jump && isGrounded)
        {
            playerController.playerStateMachine.ChangeState(new JumpState(owner));
        }
    }

    public override void Exit()
    {
        Debug.Log("Saliendo de Idle");
    }
}

public class WalkState : State
{
    public WalkState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        base.Enter();
        animator.Play("Caminata");
        Debug.Log("Entrando a Walk");
    }

    public override void Update()
    {
        base.Update();
        UpdateMovementX();

        if (InputManager.movement.x == 0f && isGrounded)
        {
            playerController.playerStateMachine.ChangeState(new IdleState(owner));
        }
        if (InputManager.jump && isGrounded)
        {
            playerController.playerStateMachine.ChangeState(new JumpState(owner));
        }
        if (!isGrounded)
        {
            playerController.playerStateMachine.ChangeState(new FallState(owner));
        }
    }

    public override void Exit()
    {
        Debug.Log("Saliendo de Walk");
    }
}

public class JumpState : State
{
    private float jumpHeight = 2f;
    private float jumpForce;
    private float buttonTime = 0.3f;
    private float jumpTime;

    public JumpState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entrando a Jump");
        jumpTime = 0;
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    public override void Update()
    {
        UpdateMovementX();

        if (InputManager.jump)
        {
            jumpTime += Time.deltaTime;
        }
        else if (jumpTime < buttonTime)
        {
            playerController.playerStateMachine.ChangeState(new FallFastState(owner));
        }

        if (rb.linearVelocity.y < 0f)
        {
            playerController.playerStateMachine.ChangeState(new FallState(owner));
        }
    }

    public override void Exit()
    {
        Debug.Log("Saliendo de Jump");
    }
}

public class FallState : State
{
    public FallState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entrando a Fall");
    }

    public override void Update()
    {
        base.Update();
        UpdateMovementX();

        if (InputManager.movement.x != 0 && isGrounded)
        {
            playerController.playerStateMachine.ChangeState(new WalkState(owner));
        }
        if (InputManager.movement.x == 0 && isGrounded)
        {
            playerController.playerStateMachine.ChangeState(new IdleState(owner));
        }
    }

    public override void Exit()
    {
        Debug.Log("Saliendo de Fall");
    }
}

public class FallFastState : FallState
{
    public FallFastState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocityY = 0f;
    }
}

public class TalkState : State
{
    public TalkState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        base.Enter();
        animator.Play("Idle");
        Debug.Log("Entrando a Talk");
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
    }

    public override void Update()
    {
        base.Update();

        if (!DialogueManager.Instance.IsDialoguePlaying)
        {
            playerController.playerStateMachine.ChangeState(new IdleState(owner));
        }
    }

    public override void Exit()
    {
        Debug.Log("Saliendo de Talk");
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}

public class AttackState : State
{
    private bool hasAttacked;

    public AttackState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        base.Enter();
        animator.Play("Attack");
        hasAttacked = false;
        Debug.Log("Entrando a Attack");
    }

    public override void Update()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Realiza el golpe una vez, a mitad de la animación
        if (!hasAttacked && stateInfo.normalizedTime >= 0.5f)
        {
            hasAttacked = true;
            DoAttack();
        }

        // Cuando termina la animación, vuelve a Idle
        if (stateInfo.normalizedTime >= 1f)
        {
            playerController.playerStateMachine.ChangeState(new IdleState(owner));
        }
    }

    private void DoAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            playerController.attackpoint.position,
            playerController.attackRange,
            playerController.enemy
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            var health = enemy.GetComponent<Health>();
            if (health != null)
                health.ChangeHealth(-playerController.attackDamage);
        }

        Debug.Log("Ataque ejecutado");
    }

    public override void Exit()
    {
        Debug.Log("Saliendo de Attack");
    }
}

