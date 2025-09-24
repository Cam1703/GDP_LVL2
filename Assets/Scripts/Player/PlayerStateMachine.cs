using UnityEngine;

public abstract class State
{
    protected GameObject owner;

    public State(GameObject owner)
        { this.owner = owner; }

    public virtual void Enter(){ }
    public virtual void Update() { }
    public virtual void Exit() { }

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
        // Debug.Log("Updating a Idle");
        if (InputManager.movement.x != 0f)
        {
            owner.GetComponent<PlayerController>().playerStateMachine.ChangeState(new WalkState(owner));
        }    
    }

    public override void Exit()
    {
        Debug.Log("Slaiendo a Idle");
    }

}

public class WalkState : State
{
    private float velocity = 5f;
    public WalkState(GameObject owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("Entrando a Walk");
    }
    public override void Update()
    {
        owner.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(InputManager.movement.x, 0f).normalized * velocity;
        
        // Debug.Log("Updating a Idle");
        if (InputManager.movement.x == 0f)
        {
            owner.GetComponent<PlayerController>().playerStateMachine.ChangeState(new IdleState(owner));
        }
    }

    public override void Exit()
    {
        Debug.Log("Slaiendo a walk");
    }
}
