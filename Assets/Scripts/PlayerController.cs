using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 6f; // Un poco más alto para que el salto se sienta bien

    [Header("Detección de Suelo")]
    public Transform controladorSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    private Rigidbody2D rb;
    private Animator animator;
    private float movimientoHorizontal;
    private bool enSuelo;

    // Como a la derecha camina bien, esto debe empezar en true
    private bool mirandoDerecha = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

        // 4. Salto con la barra espaciadora
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }

        // 5. Control de Animaciones
        animator.SetBool("Caminando", movimientoHorizontal != 0);
        animator.SetBool("Saltando", !enSuelo);

        // 6. Voltear el sprite
        // Si me muevo a la derecha (> 0) y NO estoy mirando a la derecha, voltear
        if (movimientoHorizontal > 0 && !mirandoDerecha)
        {
            Voltear();
        }
        // Si me muevo a la izquierda (< 0) y ESTOY mirando a la derecha, voltear
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
        // Invertimos el estado del booleano
        mirandoDerecha = !mirandoDerecha;

        // Multiplicamos la escala local X por -1 para girar todo el objeto
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    // Dibujo de guía en la escena
    private void OnDrawGizmos()
    {
        if (controladorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controladorSuelo.position, radioSuelo);
        }
    }
}