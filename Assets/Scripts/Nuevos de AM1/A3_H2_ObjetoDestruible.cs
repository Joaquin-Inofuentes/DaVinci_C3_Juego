using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3_H2_ObjetoDestruible : A3_Interactuable
{
    public GameObject Monedas;
    public override void Interactuar()
    {
        Monedas.SetActive(true);
        gameObject.SetActive(false);
    }

    public override void OnCollisionEnter(Collision collider)
    {
        Debug.Log(collider);
        if (collider.gameObject.layer == 9)
            Interactuar();
    }

    public override void OnDestroy()
    {
        throw new System.NotImplementedException();
    }

    /*
+MonedasASoltar : int

+Vida : int

SoltarMonedas() : void
RecibirDano() : void
*/
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
