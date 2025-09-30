using UnityEngine;

public class vidasJugador : MonoBehaviour
{
    public int vidaMax = 5;
    private int vidaActual;

    void Start()
    {
        vidaActual = vidaMax;
    }

    public void recibirDanio(int cantDanio)
    {
        vidaActual -= cantDanio;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMax);
    }

    public void recibirVida(int curaTotal)
    {
        vidaActual += curaTotal;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMax);
    }
}
