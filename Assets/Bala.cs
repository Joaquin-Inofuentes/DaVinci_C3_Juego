using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public string objetoAExcluir = "Jugador"; // Tag o nombre
    public LayerMask CapaAOmitir;
    private float timer = 0f;
    private bool impacto = false;
    public float CoeficienteDeExpansion;
    private Vector3 escalaInicial;
    private Vector3 escalaMaxima;
    // Joaco, Comentario de prueba de github. Borrarlo si, se lo ve
    void Start()
    {
        escalaInicial = transform.localScale;
        escalaMaxima = escalaInicial * CoeficienteDeExpansion; // Se expande al doble
    }

    void Update()
    {
        if (!impacto) return;

        timer += Time.deltaTime;
        float t = timer / 1f;

        // Escala con Lerp para efecto de "boom"
        transform.localScale = Vector3.Lerp(escalaMaxima, escalaInicial, t);

        if (timer >= 1f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(1);
        if (col.gameObject.layer == CapaAOmitir) return;
        if (col.gameObject.name == objetoAExcluir || col.gameObject.CompareTag(objetoAExcluir)) return;
        Debug.Log($"La {gameObject.name} impacto contra : " + col.gameObject.name);

        impacto = true;
        timer = 0f;

        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());
    }
}
