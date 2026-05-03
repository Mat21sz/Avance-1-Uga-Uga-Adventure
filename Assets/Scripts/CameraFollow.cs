using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform objetivo; // Arrastra a tu jugador aquí

    [Header("Configuración de Cámara")]
    [Range(0f, 1f)]
    public float tiempoSuavizado = 0.25f; // Qué tan suave es el seguimiento
    public Vector3 offset = new Vector3(0f, 1.5f, -10f); // Desfase para no enfocar los pies y mantener el Z en -10

    [Header("Límites del Mapa (Opcional)")]
    public bool usarLimites = false;
    public float minX, maxX;
    public float minY, maxY;

    private Vector3 velocidadReferencia = Vector3.zero;

    void LateUpdate()
    {
        // Si no hay objetivo asignado, no hacemos nada
        if (objetivo == null) return;

        // Calculamos la posición a la que queremos ir (posición del jugador + nuestro desfase)
        Vector3 posicionDeseada = objetivo.position + offset;

        // Si activamos los límites, restringimos la posición deseada
        if (usarLimites)
        {
            posicionDeseada.x = Mathf.Clamp(posicionDeseada.x, minX, maxX);
            posicionDeseada.y = Mathf.Clamp(posicionDeseada.y, minY, maxY);
        }

        // Movemos la cámara suavemente desde su posición actual hacia la deseada
        transform.position = Vector3.SmoothDamp(transform.position, posicionDeseada, ref velocidadReferencia, tiempoSuavizado);
    }
}