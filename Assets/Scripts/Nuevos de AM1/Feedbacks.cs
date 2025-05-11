using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public RawImage BarraDeVida;
    public float Vida_TamanoMaximo;
    public TextMeshProUGUI Text_CantidadDeMonedas;
    void Start()
    {
        animator = GetComponent<Animator>(); // Obtiene el Animator del GameObject
        Vida_TamanoMaximo = BarraDeVida.rectTransform.sizeDelta.y;
    }

    void Update()
    {
        // Aqu� podr�as manejar l�gica de actualizaci�n, como el movimiento
        ActualizarBarra();
        Text_CantidadDeMonedas.text = "$ " + GameManager.Componente.ContadorDeMonedas.ToString();
    }


    private void ActualizarBarra()
    {
        float AlturaActualDeLaBarraDeVida = (S_AccionesJugador.Vida / 100f) * Vida_TamanoMaximo;

        RectTransform rt = BarraDeVida.rectTransform;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, AlturaActualDeLaBarraDeVida);
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


    public RawImage feedbackImage;
    private Coroutine currentRoutine;

    
    public void FeedbackRadialVisual(Color color, float duration)
    {
        // 1. Cancela la rutina previa si existe
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        // 2. Inicia la nueva y guarda su referencia
        currentRoutine = StartCoroutine(DoFeedback(color, duration));
    }
    private IEnumerator DoFeedback(Color color, float duration)
    {
        feedbackImage.color = color;
        feedbackImage.gameObject.SetActive(true);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

            Color c = color;
            c.a = alpha;
            feedbackImage.color = c;

            yield return null;
        }

        feedbackImage.gameObject.SetActive(false);
        currentRoutine = null;
    }

}
