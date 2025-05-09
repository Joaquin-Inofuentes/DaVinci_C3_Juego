using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportarCollision : MonoBehaviour
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
        if (Emisor == null)
        {
            Debug.LogWarning("[WARNING] Emisor no asignado en CollisionInvoker.");
            return;
        }
        var Enemigo = Emisor.GetComponent<A1_Entidad>();
        if (Enemigo != null) 
        { 

        }
        var Jugador = obj.GetComponent<A1_Entidad>();
        if (Jugador != null)
            Jugador.Colisiono(Emisor, tipo);
        else
            Debug.Log($"[DEBUG] Objeto sin A1_A1_Enemigo: {obj.name} ({tipo})");
    }
}
