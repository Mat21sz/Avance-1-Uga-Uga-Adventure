using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    [Header("Objetivo a Seguir")]
    public Transform jugador;

    [Header("Ajustes de Movimiento")]
    [Range(0.01f, 1f)]
    public float velocidadSuavizado = 0.125f; // Qué tan elástica se siente la cámara
    public Vector2 offsetXY = new Vector2(0f, 1.5f); // Ajuste horizontal y vertical para centrar a Krom

    [Header("Ajustes de Vista (Perspectiva)")]
    [Tooltip("Aumenta este valor para alejar la cámara en el eje Z y ver más escenario, disminúyelo para acercarla.")]
    public float distanciaVista = 10f; // Distancia física en el eje Z hacia atrás

    void LateUpdate()
    {
        if (jugador == null) return;

        // En perspectiva, regulamos la distancia restando 'distanciaVista' en el eje Z
        Vector3 posicionDeseada = new Vector3(
            jugador.position.x + offsetXY.x,
            jugador.position.y + offsetXY.y,
            jugador.position.z - distanciaVista
        );

        // Suavizado Lerp para que la cámara no se mueva con brusquedad
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadSuavizado);
        transform.position = posicionSuavizada;
    }
}