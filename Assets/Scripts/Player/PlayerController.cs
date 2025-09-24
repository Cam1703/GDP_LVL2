using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public StateMachine playerStateMachine;

    void Awake()
    {
        playerStateMachine = new StateMachine();

        playerStateMachine.ChangeState(new IdleState(this.gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        playerStateMachine.Update();
    }
}
