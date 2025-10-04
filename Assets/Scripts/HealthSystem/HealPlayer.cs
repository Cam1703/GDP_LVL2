using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HealPlayer : MonoBehaviour
{
    public PlayerHealth playerHealth;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHealth.healHealth(1);
        }
    }
}
