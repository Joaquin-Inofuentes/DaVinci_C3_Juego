using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2_H2_Explosiva : A2_Trampa
{
    public override void Activate()
    {
        base.Activate();
        // Agregar Logica de desactivar Sonido, Apariencia, Creacion etc
    }

    public override void Desactivar()
    {
        base.Desactivar();
        // Agregar Logica de desactivar Sonido, Apariencia, Creacion etc
    }

    public override void OnCollisionEnter(Collision collider)
    {
        throw new System.NotImplementedException();
    }


    protected override void Start()
    {
        base.Start(); // Llama al Start del padre
        // C�digo propio de ArquerasElfas
    }

    protected override void Update()
    {
        base.Update(); // Llama al Update del padre
        // C�digo propio de ArquerasElfas
    }
}
