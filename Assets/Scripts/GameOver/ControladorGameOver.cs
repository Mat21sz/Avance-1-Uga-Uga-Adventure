using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorGameOver : MonoBehaviour
{
    public void ReintentarNivel()
    {
        // 1. Buscamos en la memoria el nombre de la escena que guardamos.
        // El "Nivel1" al final es un plan de emergencia por si la memoria falla o está vacía.
        string escenaACargar = PlayerPrefs.GetString("NivelGuardado", "Nivel1");

        // 2. Aseguramos que el tiempo del juego vuelva a correr normal (útil si pausaste el juego al morir)
        Time.timeScale = 1f;

        Debug.Log("Regresando a: " + escenaACargar);

        // 3. Cargamos la escena exacta donde Krom murió
        SceneManager.LoadScene(escenaACargar);
    }

    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        // Cambia "MenuPrincipal" por el nombre exacto de tu escena de inicio si la tienes
        SceneManager.LoadScene("MenuPrincipal");
    }
}