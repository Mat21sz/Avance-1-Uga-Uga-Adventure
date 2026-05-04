using UnityEngine;

public class TRexController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadCaminar = 2f;
    public float fuerzaSalto = 6f;

    [Header("Configuración de IA")]
    public float tiempoEntreMovimientos = 2f; // Cada cuánto decide reorientarse hacia el jugador
    public float tiempoEntreAtaques = 4f;     // Cada cuánto tiempo ataca obligatoriamente

    [Header("Salud del T-Rex")]
    public int vida = 10; // Golpes necesarios con la lanza para derrotarlo

    private Rigidbody2D rb;
    private Animator anim;

    private float temporizadorMovimiento;
    private float temporizadorAtaque;

    private int estadoActual = 1; // 1: Perseguir, 2: Atacar, 3: Saltar
    private float direccionHorizontal = -1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        temporizadorMovimiento = tiempoEntreMovimientos;
        temporizadorAtaque = tiempoEntreAtaques;

        IgnorarPlataformas();
        IgnorarAlJugador();
    }

    void Update()
    {
        temporizadorAtaque -= Time.deltaTime;
        temporizadorMovimiento -= Time.deltaTime;

        // 1. Prioridad: Atacar
        if (temporizadorAtaque <= 0)
        {
            EjecutarAtaque();
            temporizadorAtaque = tiempoEntreAtaques;
            temporizadorMovimiento = tiempoEntreMovimientos + 0.5f;
        }
        // 2. Prioridad: Moverse / Perseguir
        else if (temporizadorMovimiento <= 0 && estadoActual != 2)
        {
            TomarDecisionMovimiento();
            temporizadorMovimiento = tiempoEntreMovimientos;
        }

        // Aplicar la velocidad solo si está persiguiendo (1)
        if (estadoActual == 1)
        {
            rb.linearVelocity = new Vector2(velocidadCaminar * direccionHorizontal, rb.linearVelocity.y);
        }
    }

    void TomarDecisionMovimiento()
    {
        CancelInvoke("VolverACaminar");

        int probabilidad = Random.Range(0, 100);

        if (probabilidad < 70)
        {
            VolverACaminar(); // 70% de seguir caminando hacia Krom
        }
        else
        {
            EjecutarSalto();  // 30% de dar un salto impredecible
        }
    }

    void VolverACaminar()
    {
        estadoActual = 1;
        anim.SetBool("Caminar", true);
        anim.SetBool("Atacar", false);
        anim.SetBool("Saltar", false);

        // --- LÓGICA DE PERSECUCIÓN ---
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (jugador != null)
        {
            // Mira hacia dónde está Krom
            if (jugador.transform.position.x > transform.position.x)
            {
                direccionHorizontal = 1f;
            }
            else
            {
                direccionHorizontal = -1f;
            }
        }
        else
        {
            // Plan B por si el jugador ya no está
            int direccionAleatoria = Random.Range(0, 2);
            if (direccionAleatoria == 0) direccionHorizontal = 1f;
            else direccionHorizontal = -1f;
        }

        VoltearSprite();
    }

    void EjecutarAtaque()
    {
        estadoActual = 2;
        anim.SetBool("Caminar", false);
        anim.SetBool("Atacar", true);
        anim.SetBool("Saltar", false);

        // Se detiene para morder
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        Invoke("VolverACaminar", 0.5f);
    }

    void EjecutarSalto()
    {
        estadoActual = 3;
        anim.SetBool("Caminar", false);
        anim.SetBool("Atacar", false);
        anim.SetBool("Saltar", true);

        rb.linearVelocity = new Vector2(0, fuerzaSalto);

        Invoke("VolverACaminar", 0.8f);
    }

    void VoltearSprite()
    {
        Vector3 escalaLocal = transform.localScale;
        float factorVolteo = -direccionHorizontal;
        escalaLocal.x = Mathf.Abs(escalaLocal.x) * factorVolteo;
        transform.localScale = escalaLocal;
    }

    void IgnorarPlataformas()
    {
        GameObject[] plataformas = GameObject.FindGameObjectsWithTag("Plataforma");
        Collider2D[] misColliders = GetComponents<Collider2D>();

        foreach (GameObject plataforma in plataformas)
        {
            Collider2D colliderPlataforma = plataforma.GetComponent<Collider2D>();
            if (colliderPlataforma != null)
            {
                foreach (Collider2D miCollider in misColliders)
                {
                    if (!miCollider.isTrigger)
                    {
                        Physics2D.IgnoreCollision(miCollider, colliderPlataforma);
                    }
                }
            }
        }
    }

    void IgnorarAlJugador()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (jugador != null)
        {
            Collider2D colliderJugador = jugador.GetComponent<Collider2D>();
            Collider2D[] misColliders = GetComponents<Collider2D>();

            foreach (Collider2D miCollider in misColliders)
            {
                if (!miCollider.isTrigger && colliderJugador != null)
                {
                    Physics2D.IgnoreCollision(miCollider, colliderJugador);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si choca con una pared, se da la vuelta temporalmente
        if (collision.gameObject.CompareTag("Pared"))
        {
            direccionHorizontal *= -1f;
            VoltearSprite();
        }
    }

    // --- FUNCIÓN DE DAÑO (La llama el script DanoArma de la lanza) ---
    public void RecibirDanoDino()
    {
        vida--;
        Debug.Log("¡T-Rex herido por la lanza! Vida restante: " + vida);

        if (vida <= 0)
        {
            DerrotarDinosaurio();
        }
    }

    void DerrotarDinosaurio()
    {
        Debug.Log("¡VICTORIA! El T-Rex ha sido derrotado.");

        // El T-Rex desaparece
        gameObject.SetActive(false);

        // Se congela el tiempo del juego para el final épico
        Time.timeScale = 0f;
    }
}