using UnityEngine;
using UnityEngine.SceneManagement; // Esta línea es indispensable para cambiar de escena

public class ControladorCambioNivel : MonoBehaviour
{
    [Header("Configuración de Transición")]
    [Tooltip("Escribe aquí el nombre EXACTO de la escena a la que quieres ir (ej. NivelJefe)")]
    public string nombreDeLaEscena;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si el objeto que entró al sector es Krom
        if (collision.CompareTag("Jugador") || collision.CompareTag("Player"))
        {
            // Aseguramos que el tiempo del juego corra normal antes de cambiar
            Time.timeScale = 1f;

            Debug.Log("¡Krom llegó al sector final! Cargando: " + nombreDeLaEscena);

            // Cargamos la nueva escena
            SceneManager.LoadScene(nombreDeLaEscena);
        }
    }
}