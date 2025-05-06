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



    // Propiedades públicas
    public GameObject PadreDeTextDeCreditos; // Objeto donde se muestran los créditos
    public int ContadorDeMonedas; // Contador de monedas en el juego

    void Start()
    {
        // Inicialización si es necesario
    }


    // Método para iniciar la partida
    public void IniciarPartida()
    {
        // Lógica para iniciar la partida (por ejemplo, cargar la escena, resetear puntuaciones)
        Debug.Log("Partida Iniciada");
    }

    // Método para mostrar los créditos
    public void MostrarCreditos()
    {
        // Activar el GameObject que contiene los créditos
        PadreDeTextDeCreditos.SetActive(true);
    }

    // Método para ocultar los créditos
    public void OcultarCreditos()
    {
        // Desactivar el GameObject de los créditos
        PadreDeTextDeCreditos.SetActive(false);
    }

    // Método para pausar el juego y mostrar el menú
    public void PausarYMostrarMenu()
    {
        Time.timeScale = 0; // Pausar el juego
        // Aquí podrías activar una UI de menú (si tienes una)
        Debug.Log("Juego Pausado y Menú Mostrado");
    }

    // Método para reiniciar la partida
    public void Reiniciar()
    {
        // Lógica para reiniciar la partida, por ejemplo, recargar la escena actual
        Debug.Log("Partida Reiniciada");
        // Podrías usar: SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
