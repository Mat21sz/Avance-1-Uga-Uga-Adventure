using UnityEngine;

public class AtaqueEnemigo : MonoBehaviour
{
    // Usamos OnTriggerEnter2D en lugar del de colisión física
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si lo que entró en la zona invisible tiene la etiqueta "Player" (Krom)
        if (collision.CompareTag("Player"))
        {
            // Buscamos el script de salud que acabamos de hacer
            PlayerHealth saludKrom = collision.GetComponent<PlayerHealth>();

            if (saludKrom != null)
            {
                saludKrom.RecibirDano();
                Debug.Log("¡El enemigo ha dañado a Krom!");
            }
        }
    }
}