using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 6f;

    [Header("Detección de Suelo")]
    public Transform controladorSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    [Header("Combate")]
    public bool tieneLanza = false;
    public GameObject lanzaEnMano; // El objeto de la lanza que es hijo del hueso de la mano

    private Rigidbody2D rb;
    private Animator animator;
    private float movimientoHorizontal;
    private bool enSuelo;
    private bool mirandoDerecha = true;

    void Start()
    {
        // Esto garantiza que al darle Play, el universo vuelva a moverse (por si se congeló en el intento anterior)
        Time.timeScale = 1f;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Nos aseguramos de que Krom empiece con la lanza invisible (si no la ha recogido)
        if (!tieneLanza && lanzaEnMano != null)
        {
            lanzaEnMano.SetActive(false);
        }
    }

    void Update()
    {
        // 1. Detección de Inputs
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        // 2. Comprobar Suelo por Capa (Layer)
        enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioSuelo, capaSuelo);

        // 3. Comprobar Suelo por Tag "Plataforma" (Si no detectó la capa)
        if (!enSuelo)
        {
            Collider2D[] objetosBajoLosPies = Physics2D.OverlapCircleAll(controladorSuelo.position, radioSuelo);
            foreach (Collider2D obj in objetosBajoLosPies)
            {
                if (obj.CompareTag("Plataforma"))
                {
                    enSuelo = true;
                    break;
                }
            }
        }

        // 4. Salto
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }

        // 5. Ataque (con la tecla X o clic izquierdo)
        if (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Atacar");
        }

        // 6. Control de Animaciones
        animator.SetBool("Caminando", movimientoHorizontal != 0);
        animator.SetBool("Saltando", !enSuelo);

        // 7. Voltear el sprite
        if (movimientoHorizontal > 0 && !mirandoDerecha)
        {
            Voltear();
        }
        else if (movimientoHorizontal < 0 && mirandoDerecha)
        {
            Voltear();
        }
    }

    void FixedUpdate()
    {
        // El movimiento físico real
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);
    }

    private void Voltear()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    // --- FUNCIÓN PARA EQUIPAR EL ARMA AL TOCARLA EN EL SUELO ---
    public void EquiparLanza()
    {
        tieneLanza = true;

        // Hacemos visible la lanza que Krom tiene en su hueso
        if (lanzaEnMano != null)
        {
            lanzaEnMano.SetActive(true);
        }

        // Le avisamos al Animator que Krom está armado
        animator.SetBool("TieneLanza", true);

        Debug.Log("¡Krom ahora tiene la Lanza!");
    }

    // Dibujo de guía en la escena para el suelo
    private void OnDrawGizmos()
    {
        if (controladorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controladorSuelo.position, radioSuelo);
        }
    }
}