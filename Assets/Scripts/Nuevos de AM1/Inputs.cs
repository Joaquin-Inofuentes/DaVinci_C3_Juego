using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    public GameManager C_GameManager;

    // Propiedades privadas
    public KeyCode TeclaAtaque1 = KeyCode.Alpha1;
    public KeyCode TeclaAtaque2 = KeyCode.Alpha2;
    public KeyCode TeclaAtaque3 = KeyCode.Alpha3;
    public KeyCode TeclaCambioModo = KeyCode.M;
    public KeyCode botonPausa = KeyCode.Escape;

    public Vector3 movimientoMouse;

    public AccionesJugador C_AccionesJugador;

    // Update se llama una vez por frame
    void Update()
    {
        Movimiento();
        Ataque();
        Pausa();
    }
    // M�todo para mover al jugador basado en el movimiento del mouse
    public void Movimiento()
    {

        // 1. Detecta clic izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            C_AccionesJugador.IrAlDestino(GameManager.PosicionDelMouseEnElEspacio);
        }
    }

    // M�todo para realizar ataques seg�n las teclas presionadas
    public void Ataque()
    {
        if (Input.GetKeyDown(TeclaAtaque1))
        {
            C_AccionesJugador.Atacar(GameManager.PosicionDelMouseEnElEspacio, "BolaDeFuego");
            // L�gica para Ataque 1
            Debug.Log("Ataque 1 ejecutado");
        }

        if (Input.GetKeyDown(TeclaAtaque2))
        {
            // L�gica para Ataque 2
            Debug.Log("Ataque 2 ejecutado");
        }

        if (Input.GetKeyDown(TeclaAtaque3))
        {
            // L�gica para Ataque 3
            Debug.Log("Ataque 3 ejecutado");
        }

        if (Input.GetKeyDown(TeclaCambioModo))
        {
            // L�gica para cambiar de modo (por ejemplo, activar modo m�gico)
            Debug.Log("Modo cambiado");
        }
    }

    // M�todo para pausar el juego al presionar el bot�n de pausa
    public void Pausa()
    {
        if (Input.GetKeyDown(botonPausa))
        {
            // L�gica para pausar el juego
            Time.timeScale = Time.timeScale == 1 ? 0 : 1; // Alternar entre pausa y reanudaci�n
            Debug.Log("Juego pausado/despausado");
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

}
