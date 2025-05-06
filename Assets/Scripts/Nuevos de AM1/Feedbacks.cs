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
    public string TipoDeAtaque; // Ejemplo: "Físico" o "Mágico"

    void Start()
    {
        animator = GetComponent<Animator>(); // Obtiene el Animator del GameObject
    }

    void Update()
    {
        // Aquí podrías manejar lógica de actualización, como el movimiento
    }

    public void AnimacionAtaque()
    {
        // Llama a la animación de ataque
        animator.SetTrigger("Ataque"); // Suponiendo que tienes un trigger llamado "Ataque" en el Animator
    }

    public void AnimacionMovimiento()
    {
        // Llama a la animación de movimiento
        animator.SetFloat("Velocidad", 1.0f); // Suponiendo que tienes un parámetro de velocidad
    }

    public void AnimacionMuerte()
    {
        // Llama a la animación de muerte
        animator.SetTrigger("Muerte"); // Suponiendo que tienes un trigger llamado "Muerte" en el Animator
    }

    public void CreacionDeProyectiles()
    {
        // Lógica para crear proyectiles
        // Por ejemplo, instanciar un proyectil en una posición dada:
        GameObject proyectil = Instantiate(Resources.Load("ProyectilPrefab") as GameObject, transform.position, Quaternion.identity);
        // Añadir lógica de dirección, velocidad, etc., al proyectil
    }
}
