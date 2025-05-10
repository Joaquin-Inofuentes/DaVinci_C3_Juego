using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnCollisionEnter(Collision c) => ColisionoCon(c.gameObject, "CollisionEnter");
    private void OnCollisionStay(Collision c) => ColisionoCon(c.gameObject, "CollisionStay");
    private void OnCollisionExit(Collision c) => ColisionoCon(c.gameObject, "CollisionExit");

    private void OnTriggerEnter(Collider other) => ColisionoCon(other.gameObject, "TriggerEnter");
    private void OnTriggerStay(Collider other) => ColisionoCon(other.gameObject, "TriggerStay");
    private void OnTriggerExit(Collider other) => ColisionoCon(other.gameObject, "TriggerExit");




    public int danio = 10;

    private void ColisionoCon (GameObject collision, string TipoDeColision)
    {
        //Debug.Log("___" + collision.ToString() + " _ " + TipoDeColision);
        // 1. Verifica si es enemigo
        A1_Entidad enemigo = collision.gameObject.GetComponent<A1_Entidad>();
        if (enemigo != null)
        {
            enemigo.RecibirDanio(danio);
        }
        if (enemigo == null) 
        { 
            return;
        }

        // 2. Desactiva collider y arranca animación
        GetComponent<BoxCollider>().enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Destroy(rb);
        }
        StartCoroutine(AnimarYDestruir());
    }

    private IEnumerator AnimarYDestruir()
    {
        // Escala inicial
        transform.localScale = Vector3.one;

        // 3. Escalar a 2x en 0.5s
        float t = 0;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, t / 0.5f);
            yield return null;
        }

        // 4. Escalar a 0 en 1s
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.one * 2f, Vector3.zero, t / 1f);
            yield return null;
        }

        // 5. Destruir objeto
        Destroy(gameObject);
    }

}
