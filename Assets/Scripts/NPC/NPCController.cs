using UnityEngine;
public class NPCController : MonoBehaviour
{
    public NPCStateMachine npcStateMachine;
    public Transform[] patrolPoints;
    public float velocity = 2f;
    public float idleTimer = 2f;

    [HideInInspector] public int currentPatrolIndex = 0;

    private void Awake()
    {
        foreach (Transform patrolPoint in patrolPoints)
        {
            // hide patrol points in game view
            patrolPoint.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void Start()
    {
        npcStateMachine = new NPCStateMachine();
        npcStateMachine.ChangeState(new NPCIdleState(gameObject));
    }

    void Update()
    {
        npcStateMachine.Update();
    }
}

