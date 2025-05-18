using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportadorDeLasColisiones : MonoBehaviour
{
    public GameObject Emisor; // Este ser� el que se "env�a" como quien toc�

    private void OnCollisionEnter(Collision c) => ProcesarColision(c.gameObject, "CollisionEnter");
    private void OnCollisionStay(Collision c) => ProcesarColision(c.gameObject, "CollisionStay");
    private void OnCollisionExit(Collision c) => ProcesarColision(c.gameObject, "CollisionExit");

    private void OnTriggerEnter(Collider other) => ProcesarColision(other.gameObject, "TriggerEnter");
    private void OnTriggerStay(Collider other) => ProcesarColision(other.gameObject, "TriggerStay");
    private void OnTriggerExit(Collider other) => ProcesarColision(other.gameObject, "TriggerExit");

    public bool Persiguiendo = true;
    private void ProcesarColision(GameObject obj, string tipo)
    {

        if (obj.layer != 7) return;

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
            // Distancia hasta el enemigo
            float DistanciaAlEnemigo = Vector3.Distance(Enemigo.transform.position, obj.transform.position);
            // Si, Esta colisionando con el rango de ataque
            if (!tipo.Contains("Exit"))
            {
                // Si esta cerca para ataque melee
                if (DistanciaAlEnemigo < Enemigo.DistanciaParaAtaqueMelee && Enemigo.ModoAtaqueMelee == true)
                {
                    Enemigo.agent.isStopped = true;
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
                    Enemigo.agent.isStopped = false;
                    Enemigo.Objetivo = obj.gameObject;
                    Enemigo.IrAlDestino(obj.transform.position);
                }
            }
            else
            {
                Enemigo.agent.isStopped = true;
            }
            var Jugador = obj.GetComponent<A1_Entidad>();
            if (Jugador != null)
            {
                Jugador.Colisiono(Emisor, tipo);

                if (obj.GetComponent<AccionesJugador>() != null && Persiguiendo
                    &&
                    DistanciaAlEnemigo <= Enemigo.DistanciaParaPerseguir)
                {
                    obj.GetComponent<AccionesJugador>().Feedbacks.FeedbackRadialVisual
                        (
                        obj.GetComponent<AccionesJugador>().Color_FueAvistado
                        , 2f
                        );
                    Persiguiendo = false;
                }
            }
            else
            {
                //Debug.Log($"Objeto sin A1_Entidad: {obj.name} ({tipo})");
            }
        }
    }
}
