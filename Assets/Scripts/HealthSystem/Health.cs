using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    // Usamos acciones dado a que tanto el jugador como los enemigos
    // usaran el sistema pero reaccionaran de diferente forma a los
    // eventos
    public event Action OnDammaged;
    public event Action OnDeath;

    public int health;
    public int maxHealth;

    private void Awake()
    {
        health = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        health += amount;

        if(health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health <= 0)
        {
            // Death
            OnDeath?.Invoke(); 
        }
        else if (amount < 0)
        {
            //Damage takens
            OnDammaged?.Invoke();
        }
    }
}
