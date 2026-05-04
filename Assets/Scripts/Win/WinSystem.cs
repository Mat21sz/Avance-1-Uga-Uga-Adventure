using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSystem : MonoBehaviour
{
    // Este método se asignará al botón "RETRY"
    public void Reintentar()
    {
        // Imprime un mensaje en la consola
        Debug.Log("Botón Retry presionado. Cargando el nivel desde el inicio...");
        
        // Carga tu nivel 
        SceneManager.LoadScene("NivelJefe");
    }

    // Este método se asignará al botón "MENU"
    public void IrAlMenu()
    {
        // Imprime un mensaje en la consola
        Debug.Log("Botón Menu presionado. Regresando al Menú Principal...");
        
        // Carga la escena de tu menú principal
        SceneManager.LoadScene("MainMenu");
    }
}