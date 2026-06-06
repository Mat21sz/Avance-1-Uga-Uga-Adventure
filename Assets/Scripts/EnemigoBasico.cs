using UnityEngine;

public class EnemigoBasico : MonoBehaviour
{
    [Header("Configuración de Movimiento por Tiempo")]
    public float velocidad = 2f;
    [Tooltip("Tiempo en segundos que caminará hacia un lado antes de dar la vuelta")]
    public float tiempoCambioDireccion = 3f;

    [Header("Configuración de Vida")]
    public int vida = 2;

    private Rigidbody2D rb;
    private bool moviendoDerecha = false; // Asumimos que tu dibujo mira a la izquierda por defecto
    private float temporizador;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Inicializamos el reloj interno con el tiempo que elegiste
        temporizador = tiempoCambioDireccion;
    }

    void Update()
    {
        // 1. El temporizador va restando segundos en tiempo real
        temporizador -= Time.deltaTime;

        // 2. Si el tiempo llega a cero, el enemigo se da la vuelta
        if (temporizador <= 0f)
        {
            Voltear();

            // Reiniciamos el reloj para que vuelva a contar
            temporizador = tiempoCambioDireccion;
        }

        // 3. Aplicar movimiento constante dependiendo de hacia dónde mire
        float velocidadHorizontal = moviendoDerecha ? velocidad : -velocidad;
        rb.linearVelocity = new Vector2(velocidadHorizontal, rb.linearVelocity.y);
    }

    // 4. Colisiones (Para Krom y para las paredes)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Jugador") || collision.gameObject.CompareTag("Player"))
        {
            // Daño a Krom
            PlayerHealth saludKrom = collision.gameObject.GetComponent<PlayerHealth>();
            if (saludKrom != null)
            {
                saludKrom.RecibirDano();
            }
        }
        else if (collision.gameObject.CompareTag("Pared") || collision.gameObject.CompareTag("Enemigo"))
        {
            // Si choca con una pared antes de que acabe su tiempo, se da la vuelta
            Voltear();

            // Súper importante: reiniciamos el tiempo al chocar para que no se vuelva a girar enseguida
            temporizador = tiempoCambioDireccion;
        }
    }

    private void Voltear()
    {
        moviendoDerecha = !moviendoDerecha;

        // Voltea el sprite invirtiendo su escala en el eje X
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    // 5. Función para que Krom pueda matarlo
    public void RecibirDanoEnemigo()
    {
        vida--;

        if (vida <= 0)
        {
            // Destruye al enemigo
            Destroy(gameObject);
        }
    }
}