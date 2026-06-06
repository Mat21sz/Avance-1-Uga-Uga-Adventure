using UnityEngine;

public class CamaraSeguimiento2D : MonoBehaviour
{
    [Header("Objetivo a Seguir")]
    [Tooltip("Arrastra aquí a Krom")]
    public Transform objetivo;

    [Header("Configuración de Cámara")]
    [Tooltip("Tiempo que tarda la cámara en alcanzar al jugador. Menor = más rígido, Mayor = más suelto")]
    public float tiempoSuavizado = 0.25f;

    [Tooltip("Desplazamiento de la cámara. La Z DEBE ser negativa (ej. -10) para ver el nivel")]
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);

    private Vector3 velocidadActual = Vector3.zero;

    // Usamos LateUpdate para que la cámara se mueva DESPUÉS de que el jugador termine de moverse
    void LateUpdate()
    {
        if (objetivo == null) return;

        // Calculamos la posición exacta a la que la cámara debería ir (Krom + el desplazamiento)
        Vector3 posicionDeseada = objetivo.position + offset;

        // ¡EL TRUCO ORTOGRÁFICO!: Obligamos a la cámara a mantener su distancia en Z (-10)
        // para que nunca se aplaste contra los sprites 2D del mapa.
        posicionDeseada.z = offset.z;

        // Movimiento fluido usando SmoothDamp (mejor que Lerp para cámaras)
        transform.position = Vector3.SmoothDamp(transform.position, posicionDeseada, ref velocidadActual, tiempoSuavizado);
    }
}