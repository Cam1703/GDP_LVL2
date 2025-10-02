using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public abstract class State
{
    private const float distanceToCheck = 0.1f;
    protected GameObject owner;

    protected bool isGrounded;
    protected Rigidbody2D rb;
    protected PlayerController playerController;

    public State(GameObject owner)
    { 
        this.owner = owner;
        this.rb = owner.GetComponent<Rigidbody2D>();
        this.playerController = owner.GetComponent<PlayerController>();
    }

    public virtual void Enter() { }
    public virtual void Update() 
    {
        if (Physics2D.Raycast(owner.transform.position, Vector2.down, distanceToCheck, LayerMask.GetMask("Water")))
        {
            isGrounded = true;
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
        }
        else
        {
            rb.linearVelocityX = 0f;
        }
    }

}

public class StateMachine {

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
        Debug.Log("Entrando a Idle");
    }

    public override void Update()
    {
        base.Update();
        // Debug.Log("Updating a Idle");
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
        Debug.Log("Slaiendo a Idle");
    }

}

public class WalkState : State
{
    
    public WalkState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("Entrando a Walk");
    }
    public override void Update()
    {
        base.Update();
        
        UpdateMovementX();
        
        // Debug.Log("Updating a Idle");
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
        Debug.Log("Slaiendo a walk");
    }
}

public class JumpState : State
{

    public JumpState(GameObject owner) : base(owner) { }
    
    private float jumpHeight = 2f;
    private float jumpForce;
    private float buttonTime = 0.3f;
    private float jumpTime;

    public override void Enter()
    {
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
        Debug.Log("Slaiendo a jump");
    }
}

public class FallState : State
{

    public FallState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
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
        Debug.Log("Slaiendo a Fall");
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
