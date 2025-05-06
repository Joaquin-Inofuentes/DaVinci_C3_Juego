using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedbacks : MonoBehaviour
{
    /*
     AccionesJugador
     -Animator: animator
+TipoDeAtaque: string

    +AnimacionAtaque(): void
+AnimacioMovimiento(): void
+AnimacionMuerte(): void
+CreacionDeProyectiles(): void
    */

    // Propiedades
    public AccionesJugador S_AccionesJugador;
    private Animator animator;
    public string TipoDeAtaque; // Ejemplo: "F�sico" o "M�gico"

    void Start()
    {
        animator = GetComponent<Animator>(); // Obtiene el Animator del GameObject
    }

    void Update()
    {
        // Aqu� podr�as manejar l�gica de actualizaci�n, como el movimiento
    }

    public void AnimacionAtaque()
    {
        // Llama a la animaci�n de ataque
        animator.SetTrigger("Ataque"); // Suponiendo que tienes un trigger llamado "Ataque" en el Animator
    }

    public void AnimacionMovimiento()
    {
        // Llama a la animaci�n de movimiento
        animator.SetFloat("Velocidad", 1.0f); // Suponiendo que tienes un par�metro de velocidad
    }

    public void AnimacionMuerte()
    {
        // Llama a la animaci�n de muerte
        animator.SetTrigger("Muerte"); // Suponiendo que tienes un trigger llamado "Muerte" en el Animator
    }

    public void CreacionDeProyectiles()
    {
        // L�gica para crear proyectiles
        // Por ejemplo, instanciar un proyectil en una posici�n dada:
        GameObject proyectil = Instantiate(Resources.Load("ProyectilPrefab") as GameObject, transform.position, Quaternion.identity);
        // A�adir l�gica de direcci�n, velocidad, etc., al proyectil
    }
}
