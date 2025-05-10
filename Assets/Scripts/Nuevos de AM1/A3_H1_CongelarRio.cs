using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3_H1_CongelarRio : A3_Interactuable
{
    public float Duracion;
    public bool EstaCongelado;

    public override void Interactuar()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCollisionEnter(Collision collider)
    {
        throw new System.NotImplementedException();
    }

    public override void OnDestroy()
    {
        throw new System.NotImplementedException();
    }

    //+DuracionQPermaneceCongelado : float

    //+EstaCongelado : bool
    protected override void Start()
    {
        base.Start(); // Llama al Start del padre
        // Código propio de ArquerasElfas
    }
    protected override void Update()
    {
        base.Update(); // Llama al Start del padre
        // Código propio de ArquerasElfas
    }
}
