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

    public GameObject Menu;

    // Update se llama una vez por frame
    void Update()
    {
        Movimiento();
        Ataque();
        Pausa();
        //Debug.Log(Time.timeScale);
        Menu.SetActive(Time.timeScale != 1); // Alternar entre pausa y reanudaci�n
    }
    // M�todo para mover al jugador basado en el movimiento del mouse
    public void Movimiento()
    {

        // 1. Detecta clic izquierdo
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            C_AccionesJugador.IrAlDestino(GameManager.PosicionDelMouseEnElEspacio);
        }
    }

    // M�todo para realizar ataques seg�n las teclas presionadas
    public void Ataque()
    {
        // Permitir solo un ataque por frame, ignorando entradas simult�neas
        if (Input.GetKeyDown(TeclaAtaque1) &&
            !Input.GetKey(TeclaAtaque2) &&
            !Input.GetKey(TeclaAtaque3) &&
            !Input.GetKey(TeclaCambioModo))
        {
            C_AccionesJugador.Atacar(GameManager.PosicionDelMouseEnElEspacio, "BolaDeFuego");
            Debug.Log("Ataque 1 ejecutado");
        }
        else if (Input.GetKeyDown(TeclaAtaque2) &&
                 !Input.GetKey(TeclaAtaque1) &&
                 !Input.GetKey(TeclaAtaque3) &&
                 !Input.GetKey(TeclaCambioModo))
        {
            C_AccionesJugador.Atacar(GameManager.PosicionDelMouseEnElEspacio, "BolaDeHielo");
            Debug.Log("Ataque 2 ejecutado");
        }
        else if (Input.GetKeyDown(TeclaAtaque3) &&
                 !Input.GetKey(TeclaAtaque1) &&
                 !Input.GetKey(TeclaAtaque2) &&
                 !Input.GetKey(TeclaCambioModo))
        {
            C_AccionesJugador.Atacar(GameManager.PosicionDelMouseEnElEspacio, "Rayo");
            Debug.Log("Ataque 3 ejecutado");
        }
        else if (Input.GetKeyDown(TeclaCambioModo) &&
                 !Input.GetKey(TeclaAtaque1) &&
                 !Input.GetKey(TeclaAtaque2) &&
                 !Input.GetKey(TeclaAtaque3))
        {
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
            //Debug.Log("Juego pausado/despausado");
        }
    }






    // Start is called before the first frame update
    void Start()
    {

    }
    // M�todo para reanudar el juego (quitar pausa)
    public void ReanudarJuego()
    {
        Time.timeScale = 1;
        //Debug.Log("Juego reanudado");
    }

    // M�todo para salir del juego
    public void SalirDelJuego()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        Debug.Log("Saliendo del juego");
    }
}
