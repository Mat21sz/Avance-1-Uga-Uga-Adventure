using UnityEngine;
using UnityEngine.SceneManagement; // ¡SÚPER IMPORTANTE! Esto nos permite cambiar de escenas

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public int vidasActuales = 3;

    [Header("UI de Corazones")]
    public GameObject corazon3;
    public GameObject corazon2;
    public GameObject corazon1;

    public void RecibirDano()
    {
        if (vidasActuales <= 0) return;

        vidasActuales--;
        ActualizarUI();

        if (vidasActuales <= 0)
        {
            Debug.Log("¡Krom ha sido derrotado!");
            IrAGameOver(); // Llamamos a la nueva función
        }
    }

    public void Curar()
    {
        if (vidasActuales < 3)
        {
            vidasActuales++;
            ActualizarUI();
        }
    }

    private void ActualizarUI()
    {
        if (corazon3 != null) corazon3.SetActive(vidasActuales >= 3);
        if (corazon2 != null) corazon2.SetActive(vidasActuales >= 2);
        if (corazon1 != null) corazon1.SetActive(vidasActuales >= 1);
    }

    // --- NUEVA FUNCIÓN PARA CAMBIAR DE ESCENA ---
    private void IrAGameOver()
    {
        // 1. Memorizamos el nombre de la escena actual (el nivel que estamos jugando)
        string nombreEscenaActual = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("NivelGuardado", nombreEscenaActual);

        // 2. Cargamos la pantalla de Game Over
        SceneManager.LoadScene("GameOver");
    }
}