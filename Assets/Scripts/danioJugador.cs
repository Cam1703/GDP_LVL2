using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class danioJugador : MonoBehaviour
{
    public VidasJugador vidasJugador;
    private bool haceDanio = false;
    private float cooldown = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && haceDanio)
        {
            vidasJugador.recibirDanio(1);
            haceDanio = false;
            StartCoroutine(cooldownDanio());
        }
    }

   IEnumerator cooldownDanio()
    {
        yield return new WaitForSeconds(cooldown);
        haceDanio = true;
    }

}
