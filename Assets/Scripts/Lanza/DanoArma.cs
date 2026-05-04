using UnityEngine;

public class DanoArma : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si la punta de la lanza toca algo con el tag Enemigo
        if (collision.CompareTag("Enemigo"))
        {
            TRexController trex = collision.GetComponent<TRexController>();

            if (trex != null)
            {
                trex.RecibirDanoDino();
            }
        }
    }
}