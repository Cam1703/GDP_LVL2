using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HealPlayer : MonoBehaviour
{
    public PlayerHealth playerHealth;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth.healHealth(1);
        }
    }
}
