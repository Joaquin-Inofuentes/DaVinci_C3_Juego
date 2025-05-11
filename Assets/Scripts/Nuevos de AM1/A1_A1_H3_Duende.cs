using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1_A1_H3_Duende : A1_A1_Enemigo
{
    public GameObject BolaDeAtaque;
    public GameObject AtaqueActual;

    public GameObject PadreDebarraDevida;
    public GameObject BarraDeVida;

    private float anchoOriginal;
    private bool estaMuerto = false;

    void ActualizarBarraDevida()
    {
        // 1. Rotar solo en eje Y hacia la c�mara
        Vector3 camPos = Camera.main.transform.position;
        Vector3 dir = camPos - PadreDebarraDevida.transform.position;
        dir.y = 0;
        if (dir != Vector3.zero)
            PadreDebarraDevida.transform.rotation = Quaternion.LookRotation(dir);

        // 2. Calcular porcentaje real
        float porcentajeSinClamp = Vida / (float)VidaMax;
        float porcentaje = Mathf.Clamp01(porcentajeSinClamp);
        //Debug.Log($"[DEBUG] Vida: {Vida}, VidaMax: {VidaMax}, SinClamp: {porcentajeSinClamp}, Clamp01: {porcentaje}");

        // 3. Escalar ancho de la barra
        Vector3 escala = BarraDeVida.transform.localScale;
        BarraDeVida.transform.localScale = new Vector3(anchoOriginal * porcentaje, escala.y, escala.z);

        // 4. Mover localmente a la izquierda
        float offset = (anchoOriginal - (anchoOriginal * porcentaje)) / 2f;
        BarraDeVida.transform.localPosition = new Vector3(-offset, BarraDeVida.transform.localPosition.y, BarraDeVida.transform.localPosition.z);
    }

    public override void Atacar(Vector3 Destino, string Nombre = "")
    {
        if (estaMuerto) return;
        //ModoAtaqueMelee = false;
       /* if (AtaqueActual == null)
        {

            Debug.Log("Atacando");
            // Crea un efecto de danio
            //Debug.Log("Atacando");
            // Crea un efecto de da�o
            GameObject Ataque = Instantiate(BolaDeAtaque, Destino, Quaternion.identity);
            AtaqueActual = Ataque;
            Ataque.transform.localScale = new Vector3(50,50,50);
            // Destruye ese efecto
            Destroy(Ataque, 1f);
            if (ModoAtaqueMelee == true) 
            {
                animacion.SetTrigger("boss_ataque1");
            }
        }
        if (AtaqueActual != null)
        {
            //Debug.Log("Esta atacando " + gameObject, gameObject);
        }
    */      
        //Debug.Log(Nombre, gameObject);
    }

    public override void Colisiono(GameObject Colision, string TipoDeColision)
    {
        Debug.Log(Colision + " | " + TipoDeColision, gameObject);
    }

    public override void Detenerse()
    {
        Agente.isStopped = true;
       
    }

    public override void IrAlDestino(Vector3 destino)
    {
        Debug.DrawLine(destino,transform.position);
        if (estaMuerto) return;
        Agente.isStopped = false;
        Agente.SetDestination(destino);
    }

    public override void Morir()
    {
        animacion.SetBool("life", false);
        Debug.Log("Falta animacion de morir");
        StartCoroutine(DesaparecerDespuesDeSegundos(10f)); // espera 3 segundos
        if (estaMuerto) return;
        estaMuerto = true;
    }

    private IEnumerator DesaparecerDespuesDeSegundos(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        Destroy(gameObject);
    }

    public override void OnCollision(Collision collider)
    {
        throw new System.NotImplementedException();
    }

    public override void OnDisabled()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnabled()
    {
        throw new System.NotImplementedException();
    }

    public override void RecibirDanio(int cantidad)
    {
        animacion.SetTrigger("danio");
        Vida -= cantidad;
        if(Vida <= 0) 
        {
            Morir();
            animacion.SetBool("life", false);
        }
    }

    protected override void Start()
    {
        base.Start(); // Llama al Start del padre
                      // C�digo propio de ArquerasElfas
        anchoOriginal = BarraDeVida.transform.localScale.x;

    }

    protected override void Update()
    {
        base.Update(); // Llama al Update del padre
        float velocidad = Agente.velocity.magnitude;
        //Debug.Log("Velocidad agente: " + velocidad);
        animacion.SetFloat("velocidad", velocidad);
        ActualizarBarraDevida();

    }
}
