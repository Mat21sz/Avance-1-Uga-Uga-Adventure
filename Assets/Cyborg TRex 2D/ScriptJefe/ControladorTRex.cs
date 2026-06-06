using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorTRex : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadCaminar = 5f;
    public float fuerzaSalto = 12f;

    [Header("Configuración de IA")]
    public float tiempoEntreMovimientos = 2f;
    public float tiempoEntreAtaques = 5f;

    [Header("Probabilidad")]
    [Range(0, 100)]
    public int probabilidadDeSalto = 75;

    [Header("Salud del T-Rex")]
    public int vida = 10;

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
    }

    void Update()
    {
        // El reloj de ataque siempre baja
        temporizadorAtaque -= Time.deltaTime;

        // El reloj de salto/movimiento solo baja si NO está atacando
        if (estadoActual != 2)
        {
            temporizadorMovimiento -= Time.deltaTime;
        }

        // Prioridad 1: Atacar
        if (temporizadorAtaque <= 0)
        {
            EjecutarAtaque();
            temporizadorAtaque = tiempoEntreAtaques;
            // Eliminamos la línea que reseteaba el movimiento y rompía el ciclo
        }
        // Prioridad 2: Saltar o cambiar de dirección
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

        if (probabilidad < probabilidadDeSalto)
        {
            EjecutarSalto();
        }
        else
        {
            VolverACaminar();
        }
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

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        Invoke("VolverACaminar", 0.5f);
    }

    void EjecutarSalto()
    {
        estadoActual = 3;
        anim.SetBool("Caminar", false);
        anim.SetBool("Atacar", false);
        anim.SetBool("Saltar", true);

        rb.linearVelocity = new Vector2(velocidadCaminar * direccionHorizontal * 1.5f, fuerzaSalto);

        Invoke("VolverACaminar", 1.2f);
    }

    void VoltearSprite()
    {
        Vector3 escalaLocal = transform.localScale;
        float factorVolteo = -direccionHorizontal;
        escalaLocal.x = Mathf.Abs(escalaLocal.x) * factorVolteo;
        transform.localScale = escalaLocal;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pared"))
        {
            direccionHorizontal *= -1f;
            VoltearSprite();
        }
    }

    public void RecibirDanoDino()
    {
        vida--;
        Debug.Log("¡T-Rex herido! Vida: " + vida);

        if (vida <= 0)
        {
            DerrotarDinosaurio();
        }
    }

    void DerrotarDinosaurio()
    {
        Debug.Log("¡EL JUGADOR HA GANADO!");
        SceneManager.LoadScene("Win");
    }
}