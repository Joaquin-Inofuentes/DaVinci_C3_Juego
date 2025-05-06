using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class A1_Entidad : MonoBehaviour
{
    // Atributos p�blicos
    public int Vida;
    public int VidaMax;
    public int Velocidad;
    public int Da�oDeAtaque;
    public float VelocidadDeAtaque;
    public NavMeshAgent Agente;

    // M�todos abstractos (requieren implementaci�n en clases hijas)
    public abstract void IrAlDestino(Vector3 destino);
    public abstract void Detenerse();
    public abstract void RecibirDanio(int cantidad);
    public abstract void Atacar(Vector3 Destino, string Nombre = "");
    public abstract void OnCollision(Collision collider);
    public abstract void Morir();



    /*
    +Vida : int
+VidaMax : int
+Velocidad : int
+Da�oDeAtaque : int
+VelocidadDeAtaque : float
    <Abstract> IrAlDestino(Vector3): void 
<Abstract> Detenerse(): void 
<Abstract> RecibirDanio(int) : void
<Abstract> Atacar(GameObject) : void
<Abstract> OnCollision(Coll Collider) : void
<Abstract> Morir() : void


    */
}
