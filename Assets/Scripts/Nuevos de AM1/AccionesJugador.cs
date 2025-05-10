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
    private bool estaMuerto= false;

    public override void Atacar(Vector3 Destino, string Nombre)
    {
        if (estaMuerto) return;
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
            animacion.SetFloat("velocidad", 0);
            Agente.isStopped = true;
            ProyectilUsado.GetComponent<Proyectil>().danio = 15; 
        }
        if(Nombre == "Rayo") 
        {
            ProyectilUsado = Rayo;
            animacion.SetTrigger("magic3");
            animacion.SetFloat("velocidad", 0);
            Agente.isStopped = true;
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
        if (estaMuerto) return;
        Agente.isStopped = false;
        Debug.Log(1);
        transform.LookAt(destino);
        Agente.SetDestination(destino);
        Destino = destino;
        Particulas.gameObject.transform.position = destino;
        Particulas.Play();
        //Debug.Log(2);

    }
   

    public override void Morir()
    {
        if (estaMuerto) return;
        estaMuerto = true;
        animacion.SetBool("life", false);
    }

    public override void OnCollision(Collision collider)
    {
        throw new System.NotImplementedException();
    }
    public Feedbacks Feedbacks;
    public Color Color_RecibeDano;
    public Color Color_ObtieneMonedas;
    public Color Color_FueAvistado;
    public Color Color_Muere;
    public Color Color_SeCura;
    public override void RecibirDanio(int cantidad)
    {
        Vida -= cantidad;
        Debug.Log(gameObject.name + " Recibio daño de " + cantidad + " le queda " + Vida, gameObject);
        Feedbacks.FeedbackRadialVisual(Color_RecibeDano, 1);
        if (Vida <= 0) 
        {
            Morir();
            GameManager.Componente.Reiniciar();
        }
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
        float velocidadActual = Agente.velocity.magnitude;
        animacion.SetFloat("velocidad", velocidadActual);
        if (Vector3.Distance(gameObject.transform.position, Destino) < 1)
        {
            Detenerse();
           
        }
    }

    public override void Colisiono(GameObject col, string TipoDeColision)
    {
        /*
        Debug.Log(
            "El _" 
            + Colision.name  
            + "_ Colisiona con _" 
            + gameObject.name 
            + "_ Con _" 
            + TipoDeColision 
            + "_ Tipo de colision"
            , gameObject);
        */
        // El _Enemigo_ Colisiona con _Jugador v2_ Con _TriggerStay_ Tipo de colision
        A3_Interactuable interactivo = col.GetComponent<A3_Interactuable>();
        if (interactivo != null)
        {
            interactivo.Interactuar();
        }
        Debug.DrawLine(col.transform.position,gameObject.transform.position);
    }
}
