using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Aliado : MonoBehaviour
{
    #region Configuración
    [Header("Parámetros de Detección")]
    [Tooltip("Capa que identifica a los enemigos")]
    public LayerMask capaEnemigos;
    [Tooltip("Capa que identifica obstáculos (bloquean visión)")]
    public LayerMask capaObstaculos;
    [Tooltip("Intervalo entre chequeos de detección (segundos)")]
    public float intervaloDeteccion = 0.2f;
    [Tooltip("Distancia mínima para seleccionar un enemigo")]
    private float distanciaMinima = 1f;
    public int saludMaxima;
    public int salud;

    #endregion

    #region Componentes
    [Header("Componentes")]
    [Tooltip("Collider de detección (Cube con Trigger)")]
    [SerializeField] private Collider colliderDeteccion;
    [Tooltip("NavMeshAgent para mover al aliado")]
    public NavMeshAgent agente;
    #endregion

    #region Eventos
    [Header("Eventos")]
    public UnityEvent<Transform> AlCambiarObjetivo;
    #endregion

    #region Campos Privados
    private List<Transform> enemigosDetectados = new List<Transform>();
    private Transform objetivoActual;
    private float temporizador;
    #endregion

    #region Inicialización
    private void Awake()
    {
        if (colliderDeteccion == null)
            Debug.LogError("Falta asignar el collider de detección", this);
        if (!colliderDeteccion.isTrigger)
            Debug.LogWarning("El collider de detección debe estar en modo Trigger", this);
    }
    #endregion

    #region Detección en Update
    private void Update()
    {
        temporizador += Time.deltaTime;
        if (temporizador >= intervaloDeteccion)
        {
            temporizador = 0f;
            LimpiarEnemigos();
            ActualizarObjetivo();
        }
    }

    private void OnTriggerEnter(Collider otro)
    {
        if (EstaEnCapa(otro.gameObject, capaEnemigos) && !enemigosDetectados.Contains(otro.transform))
            enemigosDetectados.Add(otro.transform);
    }

    private void OnTriggerExit(Collider otro)
    {
        if (EstaEnCapa(otro.gameObject, capaEnemigos))
            enemigosDetectados.Remove(otro.transform);
    }

    private bool EstaEnCapa(GameObject obj, LayerMask capa)
    {
        return (capa & (1 << obj.layer)) != 0;
    }

    private void LimpiarEnemigos()
    {
        enemigosDetectados.RemoveAll(e => e == null);
    }

    private void ActualizarObjetivo()
    {
        Transform mejorObjetivo = null;
        float distanciaMinimaEncontrada = Mathf.Infinity;
        Vector3 posActual = transform.position;

        foreach (var enemigo in enemigosDetectados)
        {
            if (enemigo == null) continue;
            float distancia = Vector3.Distance(posActual, enemigo.position);
            if (distancia < distanciaMinimaEncontrada && distancia >= distanciaMinima && TieneLineaDeVision(enemigo))
            {
                distanciaMinimaEncontrada = distancia;
                mejorObjetivo = enemigo;
            }
        }

        if (objetivoActual != mejorObjetivo)
        {
            objetivoActual = mejorObjetivo;
            AlCambiarObjetivo?.Invoke(objetivoActual);
        }
    }

    private bool TieneLineaDeVision(Transform objetivo)
    {
        Vector3 direccion = objetivo.position - transform.position;
        if (direccion.magnitude <= Mathf.Epsilon) return true;
        return !Physics.Raycast(transform.position, direccion.normalized, direccion.magnitude, capaObstaculos);
    }
    #endregion

    #region Funcionalidades Adicionales
    // Mover a destino usando NavMeshAgent.
    // Cuando llega, se queda quieto hasta que la distancia al líder sea >= 10 metros para empezar a seguirlo.
    public void MoverADestino(Vector3 destino, Transform lider = null)
    {
        agente.SetDestination(destino);
        // Si se pasa un líder, se chequea la distancia para cambiar a seguir.
        if (lider != null)
        {
            // Se chequea una vez por Update; acá se simula la espera hasta que la distancia sea mayor.
            if (Vector3.Distance(transform.position, lider.position) >= 10f)
            {
                Debug.Log("Distancia al líder mayor a 10, cambiando a seguir al líder.");
                SeguirAlLider(lider);
            }
        }
    }

    // Seguir al líder: se actualiza el destino al líder siempre que la distancia sea >= 10 metros.
    public void SeguirAlLider(Transform lider)
    {
        if (Vector3.Distance(transform.position, lider.position) >= 10f)
        {
            agente.SetDestination(lider.position);
            Debug.Log("Siguiendo al líder: " + lider.name);
        }
        else
        {
            Debug.Log("Esperando a que el líder se aleje para seguir (menos de 10 metros).");
        }
    }

    // Disparar a discreción: dispara sin un objetivo fijo.
    public void DispararADiscrecion()
    {
        // Se puede implementar lógica de disparo aleatorio o en una dirección predeterminada.
        Debug.Log("Disparando a discreción");
    }

    // Disparar a un objetivo: rota hacia el objetivo, y si tiene línea de visión dispara; de lo contrario, se mueve a una posición con visión.
    public void DispararAObjetivo(Transform objetivo)
    {
        if (objetivo == null)
        {
            Debug.LogWarning("No hay objetivo asignado");
            return;
        }

        // Rotar para mirar al objetivo.
        Vector3 direccion = (objetivo.position - transform.position).normalized;
        if (direccion != Vector3.zero)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 5f);
        }

        // Comprobar línea de visión.
        if (TieneLineaDeVision(objetivo))
        {
            Debug.Log("Objetivo visible. Disparando a " + objetivo.name);
            // Aquí colocar la lógica real de disparo.
        }
        else
        {
            Debug.Log("Objetivo no visible. Moviéndose a posición con línea de visión.");
            IrAPosicionVisible(objetivo);
        }
    }

    // Método para moverse a una posición con línea de visión al objetivo.
    private void IrAPosicionVisible(Transform objetivo)
    {
        // Lógica simplificada: nos movemos a una posición a 2 metros detrás del objetivo en dirección opuesta.
        Vector3 direccionObjetivo = (objetivo.position - transform.position).normalized;
        Vector3 posicionDeseada = objetivo.position - direccionObjetivo * 2f;
        agente.SetDestination(posicionDeseada);
        Debug.Log("Moviéndose a posición visible: " + posicionDeseada);
    }



    public void RecibirCuracion(int VidaRecuperada)
    {
        salud += VidaRecuperada;
        if(salud > saludMaxima)
        {
            salud = saludMaxima;
        }
    }

    public void IniciarAnimacionCurar()
    {
        // Iniciar animacion de curarse
    }
    #endregion
}
