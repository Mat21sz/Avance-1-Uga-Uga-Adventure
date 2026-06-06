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
    public GameObject lanzaEnMano;
    [Tooltip("Tiempo de espera antes de poder volver a atacar (evita el bucle de sonido)")]
    public float cooldownAtaque = 0.5f;
    private float temporizadorAtaque;

    [Header("Efectos de Sonido (Acciones)")]
    public AudioSource audioSource;
    public AudioClip sfxSalto;
    public AudioClip sfxAtaque;

    [Header("Game Feel - Tiempos (Buffers)")]
    public float tiempoCoyote = 0.2f;
    private float contadorCoyote;

    public float tiempoBufferSalto = 0.2f;
    private float contadorBufferSalto;

    private Rigidbody2D rb;
    private Animator animator;

    private float movimientoHorizontal;
    private bool enSuelo;
    private bool mirandoDerecha = true;

    void Start()
    {
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (!tieneLanza && lanzaEnMano != null) lanzaEnMano.SetActive(false);
    }

    void Update()
    {
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        // --- DETECCIÓN DE SUELO ---
        enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioSuelo, capaSuelo);
        if (!enSuelo)
        {
            Collider2D[] objetosBajoLosPies = Physics2D.OverlapCircleAll(controladorSuelo.position, radioSuelo);
            foreach (Collider2D obj in objetosBajoLosPies)
            {
                if (obj.CompareTag("Plataforma") || obj.CompareTag("Enemigo"))
                {
                    enSuelo = true;
                    break;
                }
            }
        }

        if (enSuelo) contadorCoyote = tiempoCoyote;
        else contadorCoyote -= Time.deltaTime;

        // --- SALTO Y BUFFERS ---
        if (Input.GetKeyDown(KeyCode.Space)) contadorBufferSalto = tiempoBufferSalto;
        else contadorBufferSalto -= Time.deltaTime;

        if (contadorBufferSalto > 0f && contadorCoyote > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            contadorBufferSalto = 0f;
            contadorCoyote = 0f;

            if (sfxSalto != null && audioSource != null) audioSource.PlayOneShot(sfxSalto);
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            contadorCoyote = 0f;
        }

        // --- SISTEMA DE COMBATE ---
        if (temporizadorAtaque > 0)
        {
            temporizadorAtaque -= Time.deltaTime;
        }

        // ¡AQUÍ ESTÁ EL CAMBIO! Ahora exige que tieneLanza sea verdadero (true) para atacar
        if (temporizadorAtaque <= 0f && tieneLanza)
        {
            if (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("Atacar");
                temporizadorAtaque = cooldownAtaque;

                if (sfxAtaque != null && audioSource != null)
                {
                    audioSource.PlayOneShot(sfxAtaque);
                }
            }
        }

        // --- ANIMACIONES Y GIRO ---
        animator.SetBool("Caminando", movimientoHorizontal != 0);
        animator.SetBool("Saltando", !enSuelo);

        if (movimientoHorizontal > 0 && !mirandoDerecha) Voltear();
        else if (movimientoHorizontal < 0 && mirandoDerecha) Voltear();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);
    }

    private void Voltear()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    public void EquiparLanza()
    {
        tieneLanza = true;
        if (lanzaEnMano != null) lanzaEnMano.SetActive(true);
        animator.SetBool("TieneLanza", true);
    }

    private void OnDrawGizmos()
    {
        if (controladorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controladorSuelo.position, radioSuelo);
        }
    }
}