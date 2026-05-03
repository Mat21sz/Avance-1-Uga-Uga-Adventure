using UnityEngine;

public class TRexController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadCaminar = 2f;
    public float fuerzaSalto = 6f;

    [Header("Configuración de IA")]
    public float tiempoEntreAcciones = 2f;

    private Rigidbody2D rb;
    private Animator anim;
    private float temporizador;

    private int estadoActual = 1;
    private float direccionHorizontal = -1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        temporizador = tiempoEntreAcciones;
        IgnorarPlataformas();
    }

    void Update()
    {
        temporizador -= Time.deltaTime;

        if (temporizador <= 0)
        {
            TomarDecisionAleatoria();
            temporizador = tiempoEntreAcciones;
        }

        if (estadoActual == 1)
        {
            rb.linearVelocity = new Vector2(velocidadCaminar * direccionHorizontal, rb.linearVelocity.y);
        }
    }

    void TomarDecisionAleatoria()
    {
        CancelInvoke("VolverACaminar");

        int probabilidad = Random.Range(0, 100);

        // --- NUEVAS PROBABILIDADES MÁS AGRESIVAS ---
        if (probabilidad < 20)
        {
            VolverACaminar(); // 20% Caminar
        }
        else if (probabilidad < 60)
        {
            EjecutarSalto(); // 40% Saltar (del 20 al 59)
        }
        else
        {
            EjecutarAtaque(); // 40% Atacar (del 60 al 99)
        }
    }

    void VolverACaminar()
    {
        estadoActual = 1;
        anim.SetBool("Caminar", true);
        anim.SetBool("Atacar", false);
        anim.SetBool("Saltar", false);

        int direccionAleatoria = Random.Range(0, 2);
        if (direccionAleatoria == 0) direccionHorizontal = 1f;
        else direccionHorizontal = -1f;

        VoltearSprite();
    }

    void EjecutarAtaque()
    {
        estadoActual = 2;
        anim.SetBool("Caminar", false);
        anim.SetBool("Atacar", true);
        anim.SetBool("Saltar", false);

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
        Collider2D miCollider = GetComponent<Collider2D>();

        foreach (GameObject plataforma in plataformas)
        {
            Collider2D colliderPlataforma = plataforma.GetComponent<Collider2D>();
            if (colliderPlataforma != null && miCollider != null)
            {
                Physics2D.IgnoreCollision(miCollider, colliderPlataforma);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pared"))
        {
            direccionHorizontal *= -1f;
            VoltearSprite();
        }
    }
}