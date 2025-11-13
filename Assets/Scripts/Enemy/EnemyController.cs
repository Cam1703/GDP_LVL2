using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    public EnemyStateMachine enemyStateMachine;
    public Transform[] patrolPoints;
    public float velocity = 2f;
    public float idleTimer = 2f;
    public EnemyType enemyType;

    [Header("Attack Settings (Mosca only)")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackCooldown = 0.8f;

    [HideInInspector] public int currentPatrolIndex = 0;
    [HideInInspector] public bool playerInRange = false;
    [HideInInspector] public Transform player;

    private void Awake()
    {
        if (patrolPoints != null)
        {
            foreach (Transform p in patrolPoints)
            {
                var sr = p.GetComponent<SpriteRenderer>();
                if (sr != null) sr.enabled = false;
            }
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        enemyStateMachine = new EnemyStateMachine();
        enemyStateMachine.ChangeState(new EnemyIdleState(gameObject));
    }

    void Update()
    {
        enemyStateMachine.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = false;
    }
}
