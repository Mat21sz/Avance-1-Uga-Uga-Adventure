using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class MenuGameOver : MonoBehaviour
{
    // Esta función la conectaremos al ButtonRetry
    public void Reintentar()
    {
        // Recuperamos el nombre del nivel que memorizamos justo antes de morir.
        // Si por alguna razón no lo encuentra, cargará uno por defecto (cámbialo al nombre de tu nivel principal).
        string nivelARecargar = PlayerPrefs.GetString("NivelGuardado", "SampleScene");

        SceneManager.LoadScene(nivelARecargar);
    }

    // Esta función la conectaremos al ButtonMenu
    public void IrAlMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}