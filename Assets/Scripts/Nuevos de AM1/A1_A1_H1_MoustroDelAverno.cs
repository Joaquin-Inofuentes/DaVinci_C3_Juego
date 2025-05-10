using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1_A1_H1_MoustroDelAverno : A1_A1_Enemigo
{
    public GameObject BolaDeAtaque;
    public GameObject AtaqueActual;
    public override void Atacar(Vector3 Destino, string Nombre = "")
    {
        ModoAtaqueMelee = false;
        if (AtaqueActual == null)
        {
            Debug.Log("Atacando");
            // Crea un efecto de daño
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
            Debug.Log("Esta atacando " + gameObject, gameObject);
        }
      
        Debug.Log(Nombre, gameObject);
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
        Agente.isStopped = false;
        Agente.SetDestination(destino);
    }

    public override void Morir()
    {
        throw new System.NotImplementedException();
    }

    public override void MoverseAlDestino()
    {
        throw new System.NotImplementedException();
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
        Vida -= cantidad;
    }

    protected override void Start()
    {
        base.Start(); // Llama al Start del padre
        // Código propio de ArquerasElfas
    }

    protected override void Update()
    {
        base.Update(); // Llama al Update del padre
        // Código propio de ArquerasElfas
    }
}
