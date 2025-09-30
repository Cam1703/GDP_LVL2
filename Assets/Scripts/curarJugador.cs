using UnityEngine;

public class curarJugador : MonoBehaviour
{
    public VidasJugador vidasJugador;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            vidasJugador.recibirVida(1);
        }
    }
}
