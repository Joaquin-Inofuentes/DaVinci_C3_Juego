
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ControladorCamara : MonoBehaviour
{
    [Header("Configs de Apuntado")]
    public Transform Pos_Jugador; // Objeto al que sigue la cámara (el enemigo o punto de mira)
    public Vector3 Diferencia; // Objeto al que sigue la cámara (el enemigo o punto de mira)

    void FixedUpdate()
    {
        transform.position = Pos_Jugador.position - Diferencia;
        //LanzarRaycastDesdeEsquinasCanvas(); 
    }


    /* Codigo para ocultar lo q tapa la vista al jugador. Falta una lista q oculte lo q tapa y hacerlo mejor
    public RectTransform canvasRect;
    public LayerMask mascaraColision;
    void LanzarRaycastDesdeEsquinasCanvas()
    {
        Vector3[] esquinas = new Vector3[4];
        canvasRect.GetWorldCorners(esquinas); // Orden: 0=BotLeft, 1=TopLeft, 2=TopRight, 3=BotRight

        foreach (Vector3 esquina in esquinas)
        {
            Vector3 origen = esquina;
            Vector3 destino = Pos_Jugador.position;
            Vector3 direccion = (destino - origen).normalized;
            float distancia = Vector3.Distance(origen, destino);

            RaycastHit[] hits = Physics.RaycastAll(origen, direccion, distancia, mascaraColision);

            Debug.DrawRay(origen, direccion * distancia, Color.green, 0.1f);

            foreach (RaycastHit hit in hits)
            {
                Debug.Log($"[Canvas Raycast Hit] Objeto: {hit.collider.name} desde esquina: {origen}");
            }
        }
    }
    */
}

