using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using CustomInspector;

public class Enemigo : MonoBehaviour
{
    [Button(nameof(Pruebas_RecibirDano), true)] // Se usa el plugin CustomInspector
    public float CantidadDeDaño = 20; // Es lo q se le asigna al metodo para probar


    [Header("Atributos Básicos")]
    public float vidaMaxima = 250f;
    public float vidaActual;
    public float dañoNormal = 20f;
    public float dañoEspecial = 50f;
    public float velocidadMovimiento = 3.5f;
    public float rangoDeteccion = 15f;
    public float rangoAtaque = 2f;
    public float cooldownAtaque = 1.5f;
    public float cooldownAtaqueEspecial = 10f;

    [Header("Componentes")]
    public NavMeshAgent agente;
    public Transform posicionOjos;
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip sonidoAtaqueNormal;
    public AudioClip sonidoAtaqueEspecial;
    public AudioClip sonidoRecibirDanio;
    public AudioClip sonidoMorir;
    public AudioClip sonidoQTEIncremento;
    public AudioClip sonidoQTEFallo;
    public AudioClip sonidoQTEExito;

    [Header("QuickTime Event (QTE)")]
    public float tiempoQTE = 3f; // Duración total del QTE
    private float valorQTE;      // Valor actual del slider (0 a 1)
    private float timerQTE;      // Timer para el QTE
    private bool enQTE;          // Indica si se está en QTE
    private Slider sliderJugador; // Slider obtenido de los hijos del jugador

    // Probabilidad de que se active el QTE (inicial y recalculada tras cada ataque)
    private float probabilidadQTE = 0.3f;

    private Transform objetivoActual;
    private float timerAtaque;
    private float timerAtaqueEspecial;

    // Lista dinámica para almacenar objetivos detectados
    public List<Transform> listaObjetivos = new List<Transform>();

    // Para efecto de cámara básico
    private float fovOriginal;

    void Start()
    {
        vidaActual = vidaMaxima;
        agente.speed = velocidadMovimiento;
        enQTE = false;

        // Si existe cámara principal, guardar su FOV original
        if (Camera.main != null)
            fovOriginal = Camera.main.fieldOfView;
    }

    void Update()
    {
        ActualizarTimers();

        if (enQTE)
        {
            ManejarQTE();
        }
        else
        {
            if (objetivoActual == null)
            {
                RevisarObjetivos();
            }
            if (objetivoActual != null)
            {
                float distancia = Vector3.Distance(transform.position, objetivoActual.position);

                if (distancia > rangoAtaque || !EsVisible(objetivoActual))
                {
                    if (animator)
                    {
                        agente.isStopped = false;
                        agente.SetDestination(objetivoActual.position);
                        animator.SetBool("Caminando", true);
                    }
                    else
                    {
                        Debug.Log("Me falta manejar bien el NavMeshAgent");
                        MoverseAlDestinoDeLaFormaTradicional();
                    }
                }
                else
                {
                    if (animator)
                    {
                        agente.isStopped = true;
                        animator.SetBool("Caminando", false);
                    }
                    Atacar();
                }
            }
            else
            {
                if (animator)
                    animator.SetBool("Caminando", false);
            }
        }
    }

    float VelocidadDeDesplazamiento = 6f; // Ajustable
    void MoverseAlDestinoDeLaFormaTradicional()
    {
        if (objetivoActual == null) return;
        transform.position = Vector3.MoveTowards(transform.position, objetivoActual.position, VelocidadDeDesplazamiento * Time.deltaTime);
        transform.LookAt(new Vector3(objetivoActual.position.x, transform.position.y, objetivoActual.position.z));
    }


    // Actualiza timers de ataque y QTE
    void ActualizarTimers()
    {
        if (timerAtaque > 0)
            timerAtaque -= Time.deltaTime;
        if (timerAtaqueEspecial > 0)
            timerAtaqueEspecial -= Time.deltaTime;
        if (enQTE && timerQTE > 0)
            timerQTE -= Time.deltaTime;
    }





    // Cuando entre en colision 
    // Agrega a una listado de enemigos
    // En caso de ser granadaderuido. Lo pondra al comienzo de la lista desplazando el resto.
    // El metodo revisar selecciona el visible o mas cercano
    // En caso de q alguno del listado este mas lejos de lo esperado o sea null. Se elimina de la lista
    private void OnTriggerEnter(Collider other)
    {
        Transform objetivo = other.transform;

        if (!listaObjetivos.Contains(objetivo))
        {
            if (other.CompareTag("GranadaRuido"))
            {
                listaObjetivos.Insert(0, objetivo); // Prioridad máxima
            }
            if ((objetivo.CompareTag("Jugador") || objetivo.CompareTag("Aliado")) && EsVisible(objetivo.transform))
            {
                listaObjetivos.Add(objetivo);
            }
            else
            {
                Debug.Log("El objetivo " + objetivo.name + " No cumple para ser atacado");
            }
        }

        RevisarObjetivos();
    }

    private void OnTriggerExit(Collider other)
    {
        Transform objetivo = other.transform;
        if (!other.gameObject.GetComponent<DATA_Jugador>()) return;

        DATA_Jugador Player = other.gameObject.GetComponent<DATA_Jugador>();
        if (Player.salud == 0)
        {
            return;
        }

        if (listaObjetivos.Contains(objetivo))
            listaObjetivos.Remove(objetivo);

        RevisarObjetivos();
    }

    private void RevisarObjetivos()
    {
        listaObjetivos.RemoveAll(obj => obj == null || Vector3.Distance(transform.position, obj.position) > rangoDeteccion);

        Transform mejorObjetivo = null;
        float distanciaMinima = Mathf.Infinity;

        foreach (Transform posible in listaObjetivos)
        {
            if (posible.CompareTag("GranadaRuido"))
            {
                objetivoActual = posible; // Prioridad máxima
                return;
            }

            if ((posible.CompareTag("Jugador") || posible.CompareTag("Aliado")) && EsVisible(posible))
            {
                float distancia = Vector3.Distance(transform.position, posible.position);
                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    mejorObjetivo = posible;
                }
            }
        }

        objetivoActual = mejorObjetivo;
    }



    // Realiza el ataque y decide iniciar el QTE de manera aleatoria
    void Atacar()
    {
        transform.LookAt(new Vector3(objetivoActual.position.x, transform.position.y, objetivoActual.position.z));

        if (objetivoActual.GetComponent<DATA_Jugador>().salud == 0)
        {
            return;
        }

        if (timerAtaqueEspecial <= 0)
        {
            objetivoActual.GetComponent<DATA_Jugador>()?.RecibirDanio(dañoEspecial);
            timerAtaqueEspecial = cooldownAtaqueEspecial;
            if (!animator) return;
            animator.SetTrigger("AtaqueEspecial");
            audioSource.PlayOneShot(sonidoAtaqueEspecial);
        }
        else if (timerAtaque <= 0)
        {
            if (animator)
            {
                animator.SetTrigger("AtaqueNormal");
                audioSource.PlayOneShot(sonidoAtaqueNormal);
            }

            // Si el objetivo es un Jugador y la probabilidad se cumple, se inicia el QTE
            if (objetivoActual.CompareTag("Jugador") && Random.value < probabilidadQTE)
            {
                IniciarQTE();
            }
            else
            {
                objetivoActual.GetComponent<DATA_Jugador>()?.RecibirDanio(dañoNormal);
            }

            timerAtaque = cooldownAtaque;
            // Recalcula la probabilidad de QTE de forma aleatoria entre 20% y 50%
            probabilidadQTE = Random.Range(0.1f, 0.2f);
        }
    }

    // Comprueba si el objetivo es visible (línea de visión)
    bool EsVisible(Transform objetivo)
    {
        RaycastHit hit;
        Vector3 direccion = (objetivo.position + Vector3.up) - posicionOjos.position;
        if (Physics.Raycast(posicionOjos.position, direccion.normalized, out hit, rangoDeteccion))
            return hit.transform == objetivo;
        return false;
    }

    private void Pruebas_RecibirDano(float CantidadDeDaño)
    {
        // Teoricamente deberia ser el enemigo. Pero es para depurar codigo
        RecibirDanio(CantidadDeDaño, transform); 
    }


    // Recibe daño y, si no muere, asigna al atacante como objetivo
    public void RecibirDanio(float cantidad, Transform atacante)
    {
        vidaActual -= cantidad;
        audioSource.PlayOneShot(sonidoRecibirDanio);
        animator.SetTrigger("RecibirDanio");
        Debug.Log("Zombie recibió daño: " + cantidad);

        if (vidaActual <= 0)
            Morir();
        else
            objetivoActual = atacante;
    }

    // Muerte del enemigo
    void Morir()
    {
        audioSource.PlayOneShot(sonidoMorir);
        animator.SetTrigger("Morir");
        agente.isStopped = true;
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 5f);
    }

    // Inicia el QTE: activa el slider del jugador, reinicia valores y muestra feedback
    public void IniciarQTE()
    {
        // Solo si el objetivo es jugador
        if (!objetivoActual.CompareTag("Jugador"))
            return;

        enQTE = true;
        timerQTE = tiempoQTE;
        valorQTE = 0.5f;
        Debug.Log("Iniciando QTE");

        // Obtiene el slider del hijo del jugador
        sliderJugador = objetivoActual.GetComponentInChildren<Slider>();
        if (sliderJugador != null)
        {
            sliderJugador.value = valorQTE;
            sliderJugador.gameObject.SetActive(true);
            // Color inicial amarillo
            sliderJugador.fillRect.GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
        }
    }

    // Maneja la lógica del QTE: actualiza el slider, detecta entradas y cambia color
    void ManejarQTE()
    {
        valorQTE -= Time.deltaTime / tiempoQTE;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.B))
        {
            valorQTE += 0.05f;
            audioSource.PlayOneShot(sonidoQTEIncremento);
            Debug.Log("Presionaste A/B en QTE");
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        }

        // Actualiza el color del slider según el progreso
        if (sliderJugador != null)
        {
            if (valorQTE < 0.4f)
                sliderJugador.fillRect.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            else if (valorQTE > 0.8f)
                sliderJugador.fillRect.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            else
                sliderJugador.fillRect.GetComponent<UnityEngine.UI.Image>().color = Color.yellow;

            sliderJugador.value = valorQTE;
        }

        // Si llega a 1, se considera que el enemigo falló
        if (valorQTE >= 1f)
        {
            FinalizarQTE(false);
        }
        // Si llega a 0 o se agota el timer, se resta vida al jugador
        else if (valorQTE <= 0f || timerQTE <= 0f)
        {
            FinalizarQTE(true);
        }
    }

    // Finaliza el QTE, desactiva el slider y aplica efectos según el resultado
    // Si 'fallo' es true, significa que el jugador recibió daño (QTE perdido)
    // Si es false, el enemigo falló el QTE
    void FinalizarQTE(bool fallo)
    {
        enQTE = false;
        Debug.Log("Finalizando QTE. Fallo: " + fallo);
        if (sliderJugador != null)
            sliderJugador.gameObject.SetActive(false);

        if (fallo)
        {
            // Efecto de cámara: se reduce FOV para simular shake
            if (Camera.main != null)
            {
                Camera.main.fieldOfView = fovOriginal - 5;
                Invoke("RestablecerFOV", 0.2f);
            }
            audioSource.PlayOneShot(sonidoQTEFallo);
            animator.SetTrigger("FalloQTE");
            // Resta vida al jugador
            objetivoActual.GetComponent<DATA_Jugador>()?.RecibirDanio(dañoEspecial);
        }
        else
        {
            audioSource.PlayOneShot(sonidoQTEExito);
            animator.SetTrigger("EsquivadoQTE");
        }
    }

    // Restaura el Field of View original de la cámara
    void RestablecerFOV()
    {
        if (Camera.main != null)
            Camera.main.fieldOfView = fovOriginal;
    }

    // Dibuja en la escena los rangos de detección y ataque
    void OnDrawGizmosSelected()
    {
        if (objetivoActual)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(posicionOjos.position, objetivoActual.transform.position);
        }
    }
}
