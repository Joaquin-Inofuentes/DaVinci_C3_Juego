using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public string objetoAExcluir = "Jugador"; // Tag o nombre
    private float timer = 0f;
    private bool impacto = false;
    private Vector3 escalaInicial;
    private Vector3 escalaMaxima;

    void Start()
    {
        escalaInicial = transform.localScale;
        escalaMaxima = escalaInicial * 4f; // Se expande al doble
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
        Debug.Log(col.gameObject.name);
        if (col.gameObject.name == objetoAExcluir || col.gameObject.CompareTag(objetoAExcluir)) return;

        impacto = true;
        timer = 0f;

        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());
    }
}
