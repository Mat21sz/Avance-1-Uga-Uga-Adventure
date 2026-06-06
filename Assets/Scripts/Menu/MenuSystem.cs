using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    // Este método se asignará al ButtonPlay
    public void Jugar()
    {
        // Asegúrate de que el nombre "ScenaPrincipal" esté escrito exactamente igual que tu archivo de escena
        SceneManager.LoadScene("Nivel1");
    }

    // Este método se asignará a tu botón de salir
    public void Salir()
    {
        // Esto imprimirá el mensaje en la consola del editor de Unity
        Debug.Log("Saliendo del juego");
        
        // Esto cerrará el juego real una vez que lo exportes (compiles). 
        // Nota: Application.Quit() no cierra el modo "Play" dentro del editor de Unity.
        Application.Quit();
    }
}