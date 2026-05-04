using UnityEngine;

public class LanzaPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si choca con el jugador
        if (collision.CompareTag("Player"))
        {
            // Buscamos el script donde Krom maneja sus estados (aquí supongo que se llama PlayerController o similar)
            // Cambia "PlayerController" por el nombre del script de movimiento/ataque de tu jugador
            PlayerController krom = collision.GetComponent<PlayerController>();

            if (krom != null)
            {
                krom.EquiparLanza();
                Destroy(gameObject); // La lanza del suelo desaparece
            }
        }
    }
}