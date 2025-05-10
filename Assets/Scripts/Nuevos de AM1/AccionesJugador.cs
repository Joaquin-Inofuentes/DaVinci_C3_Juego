using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionesJugador : A1_Entidad
{


    public GameObject BolaDeFuego;
    public GameObject BolaDeHielo;
    public GameObject Rayo;
    public GameObject ataqueRapido;
    public GameObject Flechazo;
    
    public float fuerzaDisparo = 500f;
    public Transform Origen;

    public override void Atacar(Vector3 Destino, string Nombre)
    {
        GameObject ProyectilUsado = null;
        if(Nombre == "BolaDeFuego")
        {
            ProyectilUsado = BolaDeFuego;
            animacion.SetTrigger("magic1");
        }
        if( Nombre == "BolaDeHielo") 
        {
            ProyectilUsado = BolaDeHielo;
            animacion.SetTrigger("magic2");
            ProyectilUsado.GetComponent<Proyectil>().danio = 15; 
        }
        if(Nombre == "Rayo") 
        {
            ProyectilUsado = Rayo;
            animacion.SetTrigger("magic3");
        }
        transform.LookAt(Destino);
        Vector3 direccion = 
            (Destino - Origen.transform.position)
            .normalized;

        GameObject proyectil = Instantiate(
            ProyectilUsado, 
            Origen.transform.position, 
            Quaternion.LookRotation(direccion));

        Rigidbody rb = proyectil.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direccion * fuerzaDisparo);
        }
    }

    public override void Detenerse()
    {
        Agente.isStopped = true;
    }
    public ParticleSystem Particulas;
    public override void IrAlDestino(Vector3 destino)
    {
        Agente.isStopped = false;
        Debug.Log(1);

        transform.LookAt(destino);
     
        Agente.SetDestination(destino);
        animacion.SetFloat("velocidad", 1f);
        Destino = destino;
        Particulas.gameObject.transform.position = destino;
        Particulas.Play();
        Debug.Log(2);

    }
    IEnumerator EsperarLlegada()
    {
        Debug.Log(3);
        while (Agente.pathPending || Agente.remainingDistance > Agente.stoppingDistance + 0.1f || Agente.velocity.sqrMagnitude > 0.01f)
        {
            yield return null;
        }
        Debug.Log(4);

        // Esperar medio segundo antes de volver a idle
        yield return new WaitForSeconds(0.3f);

        Debug.Log(5);
        animacion.SetFloat("velocidad", 0f);
    }

    public override void Morir()
    {
        animacion.SetBool("live", false);
       
    }

    public override void OnCollision(Collision collider)
    {
        throw new System.NotImplementedException();
    }

    public override void RecibirDanio(int cantidad)
    {
        throw new System.NotImplementedException();
    }

    // + Agente: Navmeshagent
    // AtaqueElegido(string): void
    // GameManager
    // Inputs

    // Start is called before the first frame update
    void Start()
    {

    }
    public Vector3 Destino;
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, Destino) < 1)
        {
            Detenerse();
            animacion.SetFloat("velocidad", 0f);
        }
    }

    public override void Colisiono(GameObject Colision, string TipoDeColision)
    {
        Debug.Log(Colision + " | " + TipoDeColision, gameObject);
    }
}
