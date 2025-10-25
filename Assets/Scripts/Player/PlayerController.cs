using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocity = 5.0f;
    public StateMachine playerStateMachine;

    [Header("Attack Parameters")]
    public float attackRange = 1.5f;
    public int attackDamage = 1;
    public LayerMask enemy;
    public Transform attackpoint;

    [Header("Health Parameters")]
    public Health health;
    public SpriteRenderer spriteRenderer;

    [Header("Damage Contact")]
    public int contactDamage = 1;
    public float damageCooldown = 1f;
    private float lastDamageTime;

    void Awake()
    {
        playerStateMachine = new StateMachine();
        playerStateMachine.ChangeState(new IdleState(this.gameObject));

        if (health == null) { 
            health = GetComponent<Health>();
            PlayerHealthUI.Instance.UpdateHealthUI();

        }

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        health.OnDammaged += HandleDamage;
        health.OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        health.OnDammaged -= HandleDamage;
        health.OnDeath -= HandleDeath;
    }

    void Update()
    {
        playerStateMachine.Update();
    }

    private void HandleDamage()
    {
        PlayerHealthUI.Instance.UpdateHealthUI();
        spriteRenderer.color = Color.red;
        Invoke(nameof(ResetColor), 0.1f);
    }

    private void ResetColor()
    {
        spriteRenderer.color = Color.white;
    }

    private void HandleDeath()
    {
        Debug.Log("Jugador ha muerto");
        Destroy(gameObject);
    }

    // Detecta colisión con enemigos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colisión detectada con: " + collision.gameObject.name);
        TryDamageOnContact(collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamageOnContact(collision.gameObject);
    }

    private void TryDamageOnContact(GameObject other)
    {
        if (Time.time - lastDamageTime < damageCooldown)
            return;

        if (other.CompareTag("Enemy"))
        {
            health.ChangeHealth(-contactDamage);
            lastDamageTime = Time.time;
            Debug.Log("Jugador recibió daño por contacto con enemigo");
        }
    }
}
