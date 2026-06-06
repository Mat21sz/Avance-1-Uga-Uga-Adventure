using UnityEngine;

public class DanoArma : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Verificamos si golpeamos a un enemigo por su Tag
        if (collision.CompareTag("Enemigo"))
        {
            // 2. Prioridad A: Buscamos el script del T-Rex
            ControladorTRex trex = collision.GetComponent<ControladorTRex>();
            if (trex == null)
            {
                trex = collision.GetComponentInParent<ControladorTRex>();
            }

            // Si encontramos al T-Rex, le hacemos daño y terminamos la ejecución
            if (trex != null)
            {
                trex.RecibirDanoDino();
                Debug.Log("¡La lanza golpeó exitosamente al T-Rex!");
                return; // Esto corta la función aquí para que no busque otros scripts
            }

            // 3. Prioridad B: Buscamos el script del Enemigo Básico (Si no era el T-Rex)
            EnemigoBasico enemigoComun = collision.GetComponent<EnemigoBasico>();
            if (enemigoComun == null)
            {
                enemigoComun = collision.GetComponentInParent<EnemigoBasico>();
            }

            // Si encontramos un enemigo básico, lo eliminamos
            if (enemigoComun != null)
            {
                enemigoComun.RecibirDanoEnemigo();
                Debug.Log("¡Krom eliminó a un enemigo básico!");
            }
        }
    }
}