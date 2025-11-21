using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Health health;
    public SpriteRenderer spriteRenderer;
    private EnemyController controller;
    public int experiencePoints = 5;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    private void OnEnable()
    {
        health.OnDammaged += HandleDamage;
        health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnDammaged -= HandleDamage;
        health.OnDeath -= HandleDeath;
    }

    private void HandleDamage()
    {
        spriteRenderer.color = Color.red;
        Invoke(nameof(ResetColor), 0.1f);
        controller.enemyStateMachine.ChangeState(new EnemyDamagedState(gameObject));
    }

    private void HandleDeath()
    {
        controller.enemyStateMachine.ChangeState(new EnemyDeadState(gameObject));
        LanguageSystem.Instance.AddExperience(experiencePoints);
        Destroy(transform.root.gameObject, 1f);

    }

    private void ResetColor()
    {
        spriteRenderer.color = Color.white;
    }
}

public enum EnemyType
{
    Sapo,
    Mosca
}
