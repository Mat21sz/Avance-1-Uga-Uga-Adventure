using UnityEngine;
using UnityEngine.SceneManagement; // <--- SÚPER IMPORTANTE para cambiar de escena

public class TRexController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadCaminar = 2f;
    public float fuerzaSalto = 6f;

    [Header("Configuración de IA")]
    public float tiempoEntreMovimientos = 2f;
    public float tiempoEntreAtaques = 4f;

    [Header("Salud del T-Rex")]
    public int vida = 10;

    private Rigidbody2D rb;
    private Animator anim;
    private float temporizadorMovimiento;
    private float temporizadorAtaque;
    private int estadoActual = 1; 
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

        if (temporizadorAtaque <= 0)
        {
            EjecutarAtaque();
            temporizadorAtaque = tiempoEntreAtaques;
            temporizadorMovimiento = tiempoEntreMovimientos + 0.5f;
        }
        else if (temporizadorMovimiento <= 0 && estadoActual != 2)
        {
            TomarDecisionMovimiento();
            temporizadorMovimiento = tiempoEntreMovimientos;
        }

        if (estadoActual == 1)
        {
            rb.linearVelocity = new Vector2(velocidadCaminar * direccionHorizontal, rb.linearVelocity.y);
        }
    }

    void TomarDecisionMovimiento()
    {
        CancelInvoke("VolverACaminar");
        int probabilidad = Random.Range(0, 100);
        if (probabilidad < 70) VolverACaminar(); 
        else EjecutarSalto();  
    }

    void VolverACaminar()
    {
        estadoActual = 1;
        anim.SetBool("Caminar", true);
        anim.SetBool("Atacar", false);
        anim.SetBool("Saltar", false);

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            if (jugador.transform.position.x > transform.position.x) direccionHorizontal = 1f;
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

    // Funciones de colisión omitidas para brevedad, mantén las que ya tenías
    void IgnorarPlataformas() { /* ... Tu código anterior ... */ }
    void IgnorarAlJugador() { /* ... Tu código anterior ... */ }

    public void RecibirDanoDino()
    {
        vida--; 
        if (vida <= 0) DerrotarDinosaurio();
    }

    // --- FUNCIÓN MODIFICADA PARA CARGAR LA ESCENA WIN ---
    void DerrotarDinosaurio()
    {
        Debug.Log("¡EL JUGADOR HA GANADO!");
        
        // Cargamos la escena llamada Win
        SceneManager.LoadScene("Win");
    }
}