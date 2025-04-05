using UnityEditor.UI;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ControladorCamara : MonoBehaviour
{
    [Header("Configs de Apuntado")]
    public Transform objetivo; // Objeto al que sigue la c�mara (el enemigo o punto de mira)
    public float velocidadRotacion = 5f; // Qu� tan r�pido gira la c�mara
    public float anguloZonaMuerta = 2f; // �ngulo m�nimo para empezar a rotar
    public float temporizadorObstruccion; // Tiempo que lleva algo bloqueando la vista

    void Update()
    {
        ManejarObstruccion(); // Mueve la c�mara si hay algo bloqueando la vista

        // Calcula hacia d�nde tiene que mirar
        Vector3 direccion = objetivo.position - transform.position;
        float anguloActual = Vector3.Angle(transform.forward, direccion);

        // Si el objetivo est� fuera de la zona muerta, gira
        if (anguloActual > anguloZonaMuerta)
        {
            Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacionDeseada,
                velocidadRotacion * Time.deltaTime
            );
        }
    }

    // Detecta si hay paredes/objetos entre la c�mara y el objetivo
    void ManejarObstruccion()
    {
        RaycastHit golpe;
        // Usa un SphereCast grueso para detectar obst�culos
        if (Physics.SphereCast(transform.position, 0.2f, transform.forward, out golpe, 5f))
        {
            temporizadorObstruccion += Time.deltaTime;
            // Mueve la c�mara cerca del obst�culo
            transform.position = Vector3.Lerp(
                transform.position,
                golpe.point - transform.forward * 0.5f,
                temporizadorObstruccion
            );
        }
        else // Si no hay obst�culos, vuelve a posici�n original
        {
            temporizadorObstruccion = Mathf.Max(0, temporizadorObstruccion - Time.deltaTime);
        }
    }
}


public class SistemaMira : MonoBehaviour
{
    [Header("Visuales de la Mira")]
    public RectTransform miraUI; // Imagen de la mira en el Canvas
    public float velocidadEscala = 5f; // Rapidez al aparecer/desaparecer
    public float escalaObjetivo = 1f; // Tama�o actual deseado (0=oculta, 1=visible)
    public float temporizadorChequeo; // Cuenta regresiva para chequeos ambientales

    void Update()
    {
        // Suaviza el cambio de tama�o de la mira
        miraUI.localScale = Vector3.Lerp(
            miraUI.localScale,
            Vector3.one * escalaObjetivo,
            velocidadEscala * Time.deltaTime
        );

        // Chequea ambiente cada 0.3 segundos
        temporizadorChequeo -= Time.deltaTime;
        if (temporizadorChequeo <= 0f)
        {
            ChequearAmbiente();
            temporizadorChequeo = 0.3f;
        }
    }

    // Activa/desactiva la mira
    public void AlternarMira(bool activar)
    {
        escalaObjetivo = activar ? 1f : 0f;
    }

    // Detecta si apuntamos a algo interactuable
    void ChequearAmbiente()
    {
        RaycastHit golpe;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out golpe))
        {
            // Pone la mira roja si hay algo apuntable
            miraUI.GetComponent<RawImage>().color = golpe.collider ? Color.red : Color.white;
        }
    }
}

public class AdministradorGranadas : MonoBehaviour
{
    [Header("Configs de Lanzamiento")]
    public GameObject prefabGranada; // Modelo de la granada
    public Transform puntoLanzamiento; // Desde d�nde se tira
    public List<string> capasIgnorar = new List<string> { "Jugador" }; // Lista de capas a ignorar al lanzar (ej: jugador)
    public float temporizadorCoccion; // Tiempo que mantuvimos apretado el bot�n

    void Update()
    {
        // Mide cu�nto tiempo mantenemos el bot�n para cocinar la granada
        if (Input.GetMouseButton(1))
        {
            temporizadorCoccion += Time.deltaTime;
        }
    }

    // L�gica principal de lanzamiento
    public void LanzarGranada(Vector3 posicionObjetivo, float tiempoSostener)
    {
        // Crea la granada y le aplica fuerza
        GameObject granada = Instantiate(prefabGranada, puntoLanzamiento.position, Quaternion.identity);
        Rigidbody rb = granada.GetComponent<Rigidbody>();

        Vector3 fuerza = CalcularFuerza(posicionObjetivo, tiempoSostener);
        rb.AddForce(fuerza, ForceMode.Impulse);

        // Ignora colisi�n con capas en la lista
        foreach (string capa in capasIgnorar)
        {
            Physics.IgnoreCollision(
                granada.GetComponent<Collider>(),
                GameObject.FindGameObjectWithTag("Jugador").GetComponent<Collider>()
            );
        }
    }

    // Calcula direcci�n y fuerza del tiro
    Vector3 CalcularFuerza(Vector3 objetivo, float tiempoSostener)
    {
        // A m�s tiempo sostenido, m�s fuerte el tiro (pero con m�ximo)
        float multiplicadorFuerza = Mathf.Clamp(tiempoSostener, 1f, 3f);
        Vector3 direccion = (objetivo - puntoLanzamiento.position).normalized;
        return direccion * multiplicadorFuerza * 10f + Vector3.up * 5f; // Fuerza base + arco parab�lico
    }
}
