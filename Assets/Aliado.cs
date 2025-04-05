using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Aliado : MonoBehaviour
{
    #region Configuraci�n
    [Header("Par�metros de Detecci�n")]
    [Tooltip("Capa que identifica a los enemigos")]
    public LayerMask capaEnemigos;
    [Tooltip("Capa que identifica obst�culos (bloquean visi�n)")]
    public LayerMask capaObstaculos;
    [Tooltip("Intervalo entre chequeos de detecci�n (segundos)")]
    public float intervaloDeteccion = 0.2f;
    [Tooltip("Distancia m�nima para seleccionar un enemigo")]
    private float distanciaMinima = 1f;
    public int saludMaxima;
    public int salud;

    #endregion

    #region Componentes
    [Header("Componentes")]
    [Tooltip("Collider de detecci�n (Cube con Trigger)")]
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

    #region Inicializaci�n
    private void Awake()
    {
        if (colliderDeteccion == null)
            Debug.LogError("Falta asignar el collider de detecci�n", this);
        if (!colliderDeteccion.isTrigger)
            Debug.LogWarning("El collider de detecci�n debe estar en modo Trigger", this);
    }
    #endregion

    #region Detecci�n en Update
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
    // Cuando llega, se queda quieto hasta que la distancia al l�der sea >= 10 metros para empezar a seguirlo.
    public void MoverADestino(Vector3 destino, Transform lider = null)
    {
        agente.SetDestination(destino);
        // Si se pasa un l�der, se chequea la distancia para cambiar a seguir.
        if (lider != null)
        {
            // Se chequea una vez por Update; ac� se simula la espera hasta que la distancia sea mayor.
            if (Vector3.Distance(transform.position, lider.position) >= 10f)
            {
                Debug.Log("Distancia al l�der mayor a 10, cambiando a seguir al l�der.");
                SeguirAlLider(lider);
            }
        }
    }

    // Seguir al l�der: se actualiza el destino al l�der siempre que la distancia sea >= 10 metros.
    public void SeguirAlLider(Transform lider)
    {
        if (Vector3.Distance(transform.position, lider.position) >= 10f)
        {
            agente.SetDestination(lider.position);
            Debug.Log("Siguiendo al l�der: " + lider.name);
        }
        else
        {
            Debug.Log("Esperando a que el l�der se aleje para seguir (menos de 10 metros).");
        }
    }

    // Disparar a discreci�n: dispara sin un objetivo fijo.
    public void DispararADiscrecion()
    {
        // Se puede implementar l�gica de disparo aleatorio o en una direcci�n predeterminada.
        Debug.Log("Disparando a discreci�n");
    }

    // Disparar a un objetivo: rota hacia el objetivo, y si tiene l�nea de visi�n dispara; de lo contrario, se mueve a una posici�n con visi�n.
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

        // Comprobar l�nea de visi�n.
        if (TieneLineaDeVision(objetivo))
        {
            Debug.Log("Objetivo visible. Disparando a " + objetivo.name);
            // Aqu� colocar la l�gica real de disparo.
        }
        else
        {
            Debug.Log("Objetivo no visible. Movi�ndose a posici�n con l�nea de visi�n.");
            IrAPosicionVisible(objetivo);
        }
    }

    // M�todo para moverse a una posici�n con l�nea de visi�n al objetivo.
    private void IrAPosicionVisible(Transform objetivo)
    {
        // L�gica simplificada: nos movemos a una posici�n a 2 metros detr�s del objetivo en direcci�n opuesta.
        Vector3 direccionObjetivo = (objetivo.position - transform.position).normalized;
        Vector3 posicionDeseada = objetivo.position - direccionObjetivo * 2f;
        agente.SetDestination(posicionDeseada);
        Debug.Log("Movi�ndose a posici�n visible: " + posicionDeseada);
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
