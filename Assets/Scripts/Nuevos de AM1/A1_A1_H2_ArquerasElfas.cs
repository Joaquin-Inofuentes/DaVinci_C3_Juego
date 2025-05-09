using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1_A1_H2_ArquerasElfas : A1_A1_Enemigo
{
    public override void Atacar(Vector3 Destino, string Nombre = "")
    {
        throw new System.NotImplementedException();
    }

    public override void Colisiono(GameObject Colision, string TipoDeColision)
    {
        Debug.Log(Colision + " | " + TipoDeColision, gameObject);
    }

    public override void Detenerse()
    {
        throw new System.NotImplementedException();
    }

    public override void IrAlDestino(Vector3 destino)
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
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
