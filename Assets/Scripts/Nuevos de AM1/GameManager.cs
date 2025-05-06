using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    /*
     + PadreDeTextDeCreditos : GameObject

+ ContadorDeMonedas


    + IniciarPartida() : void
+ MostrarCreditos() : void
+ OcultarCreditos() : void
+ PausarYMostrarMenu () : void
+ Reiniciar () : void
     */



    public static GameManager COMP;



    // Propiedades p�blicas
    public GameObject PadreDeTextDeCreditos; // Objeto donde se muestran los cr�ditos
    public int ContadorDeMonedas; // Contador de monedas en el juego

    void Start()
    {
        // Inicializaci�n si es necesario
    }


    // M�todo para iniciar la partida
    public void IniciarPartida()
    {
        // L�gica para iniciar la partida (por ejemplo, cargar la escena, resetear puntuaciones)
        Debug.Log("Partida Iniciada");
    }

    // M�todo para mostrar los cr�ditos
    public void MostrarCreditos()
    {
        // Activar el GameObject que contiene los cr�ditos
        PadreDeTextDeCreditos.SetActive(true);
    }

    // M�todo para ocultar los cr�ditos
    public void OcultarCreditos()
    {
        // Desactivar el GameObject de los cr�ditos
        PadreDeTextDeCreditos.SetActive(false);
    }

    // M�todo para pausar el juego y mostrar el men�
    public void PausarYMostrarMenu()
    {
        Time.timeScale = 0; // Pausar el juego
        // Aqu� podr�as activar una UI de men� (si tienes una)
        Debug.Log("Juego Pausado y Men� Mostrado");
    }

    // M�todo para reiniciar la partida
    public void Reiniciar()
    {
        // L�gica para reiniciar la partida, por ejemplo, recargar la escena actual
        Debug.Log("Partida Reiniciada");
        // Podr�as usar: SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public static Vector3 PosicionDelMouseEnElEspacio;

    // Update is called once per frame
    void Update()
    {
        if(COMP == null)
        {
            COMP = this;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            PosicionDelMouseEnElEspacio = hit.point;
        }
    }

    public void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
