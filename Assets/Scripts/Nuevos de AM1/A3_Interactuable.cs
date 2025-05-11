using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A3_Interactuable : MonoBehaviour
{
    public AudioSource SonidoDeInteraccion;
    /*
     <abstract> OnCollisionEnter(Coll Collider): void 
<abstract> OnDestroy(): void 
     */
    protected virtual void Start()
    {
        // Código base de la trampa
    }

    protected virtual void Update()
    {
        // Código base de la trampa
    }

    public abstract void OnCollisionEnter(Collision collider);
    public abstract void OnDestroy();

    public abstract void Interactuar();

}
