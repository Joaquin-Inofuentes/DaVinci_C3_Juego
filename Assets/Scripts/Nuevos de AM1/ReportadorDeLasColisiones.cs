using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportadorDeLasColisiones : MonoBehaviour
{
    public GameObject Emisor; // Este será el que se "envía" como quien tocó

    private void OnCollisionEnter(Collision c) => ProcesarColision(c.gameObject, "CollisionEnter");
    private void OnCollisionStay(Collision c) => ProcesarColision(c.gameObject, "CollisionStay");
    private void OnCollisionExit(Collision c) => ProcesarColision(c.gameObject, "CollisionExit");

    private void OnTriggerEnter(Collider other) => ProcesarColision(other.gameObject, "TriggerEnter");
    private void OnTriggerStay(Collider other) => ProcesarColision(other.gameObject, "TriggerStay");
    private void OnTriggerExit(Collider other) => ProcesarColision(other.gameObject, "TriggerExit");

    private void ProcesarColision(GameObject obj, string tipo)
    {
        if (obj.name == Emisor.name) 
        { 
            return;
        }
        if (Emisor == null)
        {
            Debug.LogWarning("[WARNING] Emisor no asignado en CollisionInvoker.");
            return;
        }
        var Enemigo = Emisor.GetComponent<A1_Entidad>();
        if (Enemigo != null)
        {
            // Si, Esta colisionando con el rango de ataque
            if (!tipo.Contains("Exit"))
            {
                // Distancia hasta el enemigo
                float DistanciaAlEnemigo = Vector3.Distance(Enemigo.transform.position, obj.transform.position);
                // Si esta cerca para ataque melee
                if (DistanciaAlEnemigo < Enemigo.DistanciaParaAtaqueMelee && Enemigo.ModoAtaqueMelee == true)
                {
                    Enemigo.Agente.isStopped = true;
                    Enemigo.Atacar(obj.transform.position, "Melee");
                }

                if (DistanciaAlEnemigo < Enemigo.DistanciaParaAtaqueMelee && Enemigo.ModoAtaqueMelee == true)
                {
                    Enemigo.Agente.isStopped = true;
                    Enemigo.Atacar(obj.transform.position, "Melee");
                }

                // Si esta debo acercarme
                if (
                    (
                    (DistanciaAlEnemigo >= Enemigo.DistanciaParaAtaqueMelee && Enemigo.ModoAtaqueMelee == true)
                    ||
                    (DistanciaAlEnemigo >= Enemigo.DistanciaParaAtaqueLargo && Enemigo.ModoAtaqueMelee == false)
                    )
                    && 
                    DistanciaAlEnemigo <= Enemigo.DistanciaParaPerseguir
                    )
                {
                    Enemigo.Agente.isStopped = false;
                    Enemigo.IrAlDestino(obj.transform.position);
                }
            }
            else
            {
                Enemigo.Agente.isStopped = true;
            }
        }
        var Jugador = obj.GetComponent<A1_Entidad>();
        if (Jugador != null)
            Jugador.Colisiono(Emisor, tipo);
        else
        {
            //Debug.Log($"Objeto sin A1_Entidad: {obj.name} ({tipo})");
        }
    }
}
