using UnityEditor.UI;
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
    }
}

