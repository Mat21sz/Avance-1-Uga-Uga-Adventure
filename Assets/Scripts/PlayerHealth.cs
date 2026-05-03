using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public int vidasActuales = 3;

    [Header("UI de Corazones")]
    // Aquí arrastrarás las imágenes de tu Canvas en el Inspector
    public GameObject corazon3;
    public GameObject corazon2;
    public GameObject corazon1;

    // Este método DEBE ser público para que el T-Rex pueda llamarlo desde su Visual Script
    public void RecibirDano()
    {
        // Si ya no tiene vidas, no hacemos nada
        if (vidasActuales <= 0) return;

        // Restamos una vida
        vidasActuales--;

        // Actualizamos la interfaz
        ActualizarUI();

        // Comprobamos si Krom ha perdido todas sus vidas
        if (vidasActuales <= 0)
        {
            Debug.Log("¡Krom ha sido derrotado por el T-Rex!");
            // Aquí en el futuro puedes poner la lógica para reiniciar el nivel
        }
    }

    private void ActualizarUI()
    {
        // Apagamos los corazones dependiendo de la vida actual
        if (vidasActuales < 3 && corazon3 != null) corazon3.SetActive(false);
        if (vidasActuales < 2 && corazon2 != null) corazon2.SetActive(false);
        if (vidasActuales < 1 && corazon1 != null) corazon1.SetActive(false);
    }
}