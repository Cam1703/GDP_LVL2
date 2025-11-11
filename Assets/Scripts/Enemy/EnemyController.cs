using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyStateMachine enemyStateMachine;
    public Transform[] patrolPoints;
    public float velocity = 2f;
    public float idleTimer = 2f;
    public EnemyType enemyType;

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
        enemyStateMachine = new EnemyStateMachine();
        enemyStateMachine.ChangeState(new EnemyIdleState(gameObject));
    }

    void Update()
    {
        enemyStateMachine.Update();
    }
}
