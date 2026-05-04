using UnityEngine;

public class BistecCurativo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si choca con el jugador
        if (collision.CompareTag("Player"))
        {
            PlayerHealth salud = collision.GetComponent<PlayerHealth>();

            if (salud != null)
            {
                // Solo se consume el bistec si a Krom le falta vida
                if (salud.vidasActuales < 3)
                {
                    salud.Curar();
                    Destroy(gameObject); // El bistec desaparece
                }
            }
        }
    }
}