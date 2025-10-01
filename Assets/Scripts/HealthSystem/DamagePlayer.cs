using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public PlayerHealth playerHealth;
    private bool doDamage = true;
    private float cooldown = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && doDamage)
        {
            playerHealth.recieveDamage(1);
            doDamage = false;
            StartCoroutine(cooldownDamage());
        }
    }

   IEnumerator cooldownDamage()
    {
        yield return new WaitForSeconds(cooldown);
        doDamage = true;
    }

}
