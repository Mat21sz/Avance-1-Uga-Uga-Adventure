using UnityEngine;

public class TrampaPuas : MonoBehaviour
{
    // Funciona por si las púas son sólidas (Krom choca contra ellas)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AtacarKrom(collision.gameObject);
        }
    }

    // Funciona por si las púas son "fantasmas" (Is Trigger está activado)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AtacarKrom(collision.gameObject);
        }
    }

    private void AtacarKrom(GameObject jugador)
    {
        // Buscamos el script exacto que creamos para la vida de Krom
        PlayerHealth saludKrom = jugador.GetComponent<PlayerHealth>();

        if (saludKrom != null)
        {
            // Llamamos a tu función, que ya se encarga de quitar 1 vida, sonar y dar invulnerabilidad
            saludKrom.RecibirDano();
            Debug.Log("¡Krom pisó las púas!");
        }
    }
}