using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections; // Necesario para el parpadeo de invulnerabilidad

public class PlayerHealth : MonoBehaviour
{
    [Header("Efectos de Sonido (SFX)")]
    public AudioSource audioSource;
    public AudioClip sfxRecibirDano;

    [Header("Configuración de Vida")]
    public int vidasActuales = 3;
    public int vidaMaxima = 3;

    [Header("Invulnerabilidad")]
    public float tiempoInvulnerabilidad = 1.5f; // Tiempo que Krom es intocable tras un golpe
    private bool esInvulnerable = false;
    private SpriteRenderer spriteRenderer;

    [Header("UI de Corazones")]
    public GameObject corazon3;
    public GameObject corazon2;
    public GameObject corazon1;

    [Header("Escena de Derrota")]
    public string nombreEscenaDerrota = "GameOver"; // Escribe aquí el nombre exacto de tu escena de perder

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtenemos el dibujo de Krom para hacerlo parpadear

        ActualizarUI();
    }

    public void RecibirDano()
    {
        // Si Krom ya murió o si está en su periodo de invulnerabilidad, no le hacemos daño
        if (vidasActuales <= 0 || esInvulnerable) return;

        vidasActuales--;

        if (sfxRecibirDano != null && audioSource != null)
        {
            audioSource.PlayOneShot(sfxRecibirDano);
        }

        ActualizarUI();

        if (vidasActuales <= 0)
        {
            Morir();
        }
        else
        {
            // Si sobrevivió al golpe, activamos la invulnerabilidad
            StartCoroutine(RutinaInvulnerabilidad());
        }
    }

    // --- CORRUTINA DE PARPADEO (I-FRAMES) ---
    private IEnumerator RutinaInvulnerabilidad()
    {
        esInvulnerable = true;

        // Hacemos que Krom parpadee en color rojo unas cuantas veces
        for (int i = 0; i < 3; i++)
        {
            if (spriteRenderer != null) spriteRenderer.color = new Color(1f, 0f, 0f, 0.5f); // Rojo semi-transparente
            yield return new WaitForSeconds(tiempoInvulnerabilidad / 6f);

            if (spriteRenderer != null) spriteRenderer.color = Color.white; // Vuelve a la normalidad
            yield return new WaitForSeconds(tiempoInvulnerabilidad / 6f);
        }

        esInvulnerable = false; // Vuelve a ser vulnerable
    }

    public void Curar()
    {
        if (vidasActuales < vidaMaxima)
        {
            vidasActuales++;
            ActualizarUI();
            Debug.Log("¡Krom comió el bistec y se curó!");
        }
    }

    private void ActualizarUI()
    {
        if (corazon3 != null) corazon3.SetActive(vidasActuales >= 3);
        if (corazon2 != null) corazon2.SetActive(vidasActuales >= 2);
        if (corazon1 != null) corazon1.SetActive(vidasActuales >= 1);
    }

    private void Morir()
    {
        Debug.Log("¡Krom ha sido derrotado!");
        string nombreEscenaActual = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("NivelGuardado", nombreEscenaActual);
        PlayerPrefs.Save();
        SceneManager.LoadScene(nombreEscenaDerrota);
    }
}