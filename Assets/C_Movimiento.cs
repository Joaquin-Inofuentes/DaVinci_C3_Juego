using UnityEngine;

/// <summary>
/// Controlador de movimiento del jugador para desplazarse por el nivel
/// </summary>
public class C_Movimiento : MonoBehaviour // Controlador de movimiento
{
    public float Velocidad = 5f;
    public float VelMinima = 5f;
    public float VelMaxima = 7f;
    public float fuerzaSalto = 7f;
    public Transform Jugador;
    public LayerMask Suelo;
    public Rigidbody rb;
    public Transform Arma;

    public float inputX, inputZ;
    public bool enSuelo;
    public bool movimientoBloqueado = false;

    public DATA_Jugador DataJugador;

    public void Start()
    {
        DataJugador = GetComponent<DATA_Jugador>();
    }

    void Update()
    {
        // Se definen los inputs
        inputX = Input.GetAxisRaw("Horizontal");
        inputZ = Input.GetAxisRaw("Vertical");

        // Se escuchan los inputs de teclado
        EntradasDeTeclado();

        // El mago siempre mirara al cursor
        ElJugadorMiraAlCursor();
        ElArmaApuntaAlCursor(DataJugador.armaEquipada.OrigenDeDisparos.transform,DataJugador.armaEquipada.PrefabDeArma.transform);

        // Funciones de movimiento
        Moverse();


        #region // Esto es por q antes era un FPS
        //RotacionEnEjeYParaCamara();

        // Configuracion extra
        if (Input.GetKeyDown(KeyCode.F4))
        {
            // IterarBloqueoDeCursor();
        }
        #endregion
    }

    public void ElJugadorMiraAlCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(targetPos);
        }
    }

    public void ElArmaApuntaAlCursor(Transform origenDisparo, Transform arma)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // EXCLUIR Layer 6 (Enemigos)
        int capaEnemigos = 6;
        int mascaraInversa = ~(1 << capaEnemigos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mascaraInversa))
        {
            Vector3 direccion = (hit.point - origenDisparo.position).normalized;
            Debug.DrawLine(origenDisparo.position, hit.point);
            arma.rotation = Quaternion.LookRotation(direccion);
        }
    }







    // Entradas de teclado de movimiento
    public void EntradasDeTeclado()
    {
        // ___ Desplazamiento del jugador
        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            Saltar();
        }


        // Switch de agacharse o tumbarse
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Tumbarse();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Agacharse();
        }

        // Manteniendo el agacharse
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Agacharse();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Levantarse();
        }

        // Correr
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Velocidad = VelMaxima;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Velocidad = VelMinima;
        }

    }

    public void RotacionEnEjeYParaCamara()
    {
        if (!CursorDentroPantalla()) return;

        float rotacionY = Input.GetAxis("Mouse X") * 3f; // sensibilidad
        transform.Rotate(0, rotacionY, 0);
    }

    public bool CursorDentroPantalla()
    {
        return Screen.safeArea.Contains(Input.mousePosition);
    }



    void Tumbarse()
    {
        if (Jugador.transform.localScale == new Vector3(1, 1, 1))
        {
            Jugador.transform.localScale = new Vector3(1, 0.4f, 1);
            return;
        }
        else
        {
            Levantarse();
        }
    }

    void Agacharse()
    {
        if (Jugador.transform.localScale == new Vector3(1, 1, 1))
        {
            Jugador.transform.localScale = new Vector3(1, 0.7f, 1);
            Velocidad = VelMinima - 3;
            return;
        }
        else
        {
            Levantarse();
        }
    }

    void Levantarse()
    {
        Velocidad = VelMinima;
        Jugador.transform.localScale = new Vector3(1, 1, 1);
    }


    void Moverse()
    {
        if (movimientoBloqueado) return;

        Vector3 direccion = new Vector3(inputX, 0, inputZ);
        
        transform.Translate(direccion * Velocidad * Time.deltaTime, Space.World);

        // FPS
        //Vector3 movimiento = transform.TransformDirection(direccion) * Velocidad * Time.deltaTime;
        //transform.position += movimiento;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            enSuelo = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            enSuelo = false;
        }
    }

    void Saltar()
    {
        rb.velocity = new Vector3(rb.velocity.x, fuerzaSalto, rb.velocity.z);
    }

    public void IterarBloqueoDeCursor()
    {
        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

}
