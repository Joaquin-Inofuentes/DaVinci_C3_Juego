using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A2_Trampa : MonoBehaviour
{

    /*
     #activada: bool 
     <abstract> OnCollisionEnter(Coll Collider): void 
     * 
     */
    protected bool Activado;
    public virtual void Activate()
    {
        Activado = true;
    }
    public virtual void Desactivar()
    {
        Activado = false;
    }

    public abstract void OnCollisionEnter(Collision collider);


    protected virtual void Start()
    {
        // C�digo base de la trampa
    }

    protected virtual void Update()
    {
        // C�digo base de la trampa
    }

}
