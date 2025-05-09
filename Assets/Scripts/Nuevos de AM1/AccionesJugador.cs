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
            animacion.SetBool("atacando", true);
        }
        else if( Nombre == "BolaDeHielo") 
        {
            ProyectilUsado = BolaDeHielo;
            ProyectilUsado.GetComponent<Proyectil>().danio = 15; 
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
        transform.LookAt(destino);
        Debug.Log(destino);
        //Debug.Log(Agente);
        Agente.SetDestination(destino);
        Destino = destino;
        Particulas.gameObject.transform.position = destino;
        Particulas.Play();
<<<<<<< HEAD
<<<<<<< HEAD

        StartCoroutine(EsperarLlegada());
=======
>>>>>>> parent of 7b567d2 (cambios part 2)
=======
        float velocidad = new Vector3(GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.z).magnitude;
        animator.SetFloat("velocidad", velocidad);
>>>>>>> parent of f9429c8 (animaciones part 3)
    }

    public override void Morir()
    {
        throw new System.NotImplementedException();
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
        }
    }
}
