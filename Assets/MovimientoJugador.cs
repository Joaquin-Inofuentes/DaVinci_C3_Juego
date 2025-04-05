using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float Velocidad = 5f;
    public float VelMinima = 5f;
    public float VelMaxima = 7f;
    public float fuerzaSalto = 7f;
    public Transform Jugador;
    public LayerMask Suelo;
    public Rigidbody rb;

    public float inputX, inputZ;
    public bool enSuelo;
    public bool movimientoBloqueado = false;

    void Update()
    {
        // Se definen los inputs
        inputX = Input.GetAxisRaw("Horizontal");
        inputZ = Input.GetAxisRaw("Vertical");

        // Se escuchan los inputs de teclado
        EntradasDeTeclado();

        // Funciones de movimiento
        Moverse();
        RotacionEnEjeYParaCamara();

        // Configuracion extra
        if (Input.GetKeyDown(KeyCode.F4))
        {
            IterarBloqueoDeCursor();
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

        Vector3 direccion = new Vector3(inputX, 0, inputZ).normalized;
        Vector3 movimiento = transform.TransformDirection(direccion) * Velocidad * Time.deltaTime;
        transform.position += movimiento;
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
