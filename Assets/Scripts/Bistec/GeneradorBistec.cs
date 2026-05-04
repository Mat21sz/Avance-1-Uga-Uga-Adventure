using UnityEngine;

public class GeneradorBistec : MonoBehaviour
{
    [Header("Configuración del Generador")]
    public GameObject bistecPrefab;
    public float tiempoParaReaparecer = 5f;

    [Header("Configuración del Bistec")]
    public float tiempoDeVidaBistec = 6f; // Segundos antes de que el bistec desaparezca solo

    private GameObject bistecActual;
    private float temporizador;

    void Start()
    {
        temporizador = 1f;
    }

    void Update()
    {
        // Si NO hay bistec (porque te lo comiste O porque desapareció por tiempo)
        if (bistecActual == null)
        {
            temporizador -= Time.deltaTime;

            if (temporizador <= 0)
            {
                GenerarBistec();
                temporizador = tiempoParaReaparecer;
            }
        }
    }

    void GenerarBistec()
    {
        GameObject[] puntos = GameObject.FindGameObjectsWithTag("PuntoBistec");

        if (puntos.Length == 0)
        {
            Debug.LogError("¡Faltan los Spawn Points! Crea objetos vacíos y ponles el Tag 'PuntoBistec'.");
            return;
        }

        int indiceAleatorio = Random.Range(0, puntos.Length);
        Transform puntoElegido = puntos[indiceAleatorio].transform;

        // Instanciamos el bistec EXACTAMENTE en la posición de ese punto
        bistecActual = Instantiate(bistecPrefab, puntoElegido.position, Quaternion.identity);

        // --- LA MAGIA ESTÁ AQUÍ ---
        // Le decimos a Unity: "Destruye este bistec exactamente en 'tiempoDeVidaBistec' segundos"
        Destroy(bistecActual, tiempoDeVidaBistec);

        Debug.Log("Bistec servido en el punto: " + puntoElegido.name + " (Desaparecerá en " + tiempoDeVidaBistec + "s)");
    }
}