using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Health health;
    public SpriteRenderer spriteRenderer;

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
        Invoke("ResetColor", 0.1f);
    }

    private void HandleDeath()
    {
        Destroy(gameObject);
    }

    private void ResetColor()
    {
        spriteRenderer.color = Color.white;
    }
}
