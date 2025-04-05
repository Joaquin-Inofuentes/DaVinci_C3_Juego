using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // Para reiniciar la partida
using System.Collections.Generic;

public class Mando : MonoBehaviour
{
    // Lista dinámica de Aliados de la escuadra
    public List<GameObject> listaAliados = new List<GameObject>();
    public GameObject AliadoActual;           // Aliado controlado actualmente
    public GameObject AliadoPrefab;           // Prefab para instanciar nuevos Aliados

    // Estado de la partida
    public bool partidaPausada = false;

    // UI para feedback
    public Text mensajeUI;                     // Texto para mostrar mensajes en pantalla
    public GameObject menuPausa;               // Panel de menú de pausa

    // Audio para feedback
    public AudioSource audioSource;
    public AudioClip clipCambioAliado;
    public AudioClip clipReiniciar;
    public AudioClip clipPausar;
    public AudioClip clipMovimiento;
    public AudioClip clipSeguirLider;
    public AudioClip clipCurarAliado;
    public AudioClip clipAgregarAliado;
    public AudioClip clipQuitarAliado;

    void Start()
    {
        // Si la lista está vacía, se asume que ya hay Aliados en la escena
        if (listaAliados.Count == 0)
        {
            // Se busca en la escena todos los Aliados con la etiqueta "Aliado"
            GameObject[] AliadosEnEscena = GameObject.FindGameObjectsWithTag("Aliado");
            listaAliados.AddRange(AliadosEnEscena);
        }
        // Asigna el primer Aliado de la lista como el controlado
        if (listaAliados.Count > 0)
        {
            AliadoActual = listaAliados[0];
            ActualizarUI("Controlando: " + AliadoActual.name);
        }
    }

    void Update()
    {
        // Ejemplo de controles para probar las funciones (pueden adaptarse a inputs de UI)
        if (Input.GetKeyDown(KeyCode.C))
        {
            CambiarDeAliado();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReiniciarPartida();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PausarPartida();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Ordenar movimiento a un destino fijo (por ejemplo, posición (10, 0, 10))
            Vector3 destino = new Vector3(10, 0, 10);
            OrdenarMovimiento(destino);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            OrdenarSeguirAlLider();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            OrdenarCurarAliado();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AgregarAliado();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            QuitarAliado(AliadoActual);
        }
    }

    // Cambia el Aliado controlado al siguiente de la lista
    public void CambiarDeAliado()
    {
        if (listaAliados.Count > 1)
        {
            int indiceActual = listaAliados.IndexOf(AliadoActual);
            int nuevoIndice = (indiceActual + 1) % listaAliados.Count;
            AliadoActual = listaAliados[nuevoIndice];
            ActualizarUI("Cambiado a: " + AliadoActual.name);
            audioSource.PlayOneShot(clipCambioAliado);
        }
        else
        {
            ActualizarUI("Solo hay un Aliado.");
        }
    }

    // Reinicia la partida (recargando la escena actual)
    public void ReiniciarPartida()
    {
        ActualizarUI("Reiniciando partida...");
        audioSource.PlayOneShot(clipReiniciar);
        // Reinicia Time.timeScale por si estaba pausado
        Time.timeScale = 1;
        partidaPausada = false;
        if (menuPausa != null)
            menuPausa.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Pausa o reanuda la partida
    public void PausarPartida()
    {
        if (!partidaPausada)
        {
            partidaPausada = true;
            Time.timeScale = 0;
            if (menuPausa != null)
                menuPausa.SetActive(true);
            ActualizarUI("Partida pausada");
            audioSource.PlayOneShot(clipPausar);
        }
        else
        {
            partidaPausada = false;
            Time.timeScale = 1;
            if (menuPausa != null)
                menuPausa.SetActive(false);
            ActualizarUI("Partida reanudada");
        }
    }

    // Ordena al Aliado actual moverse hacia un destino determinado
    public void OrdenarMovimiento(Vector3 destino)
    {
        if (AliadoActual != null)
        {
            Aliado comp = AliadoActual.GetComponent<Aliado>();
            if (comp != null)
            {
                comp.MoverADestino(destino);
                ActualizarUI(AliadoActual.name + " se mueve a " + destino.ToString());
                audioSource.PlayOneShot(clipMovimiento);
            }
        }
    }

    // Ordena a toda la escuadra (todos los Aliados) seguir al líder (definido como el primer Aliado de la lista)
    public void OrdenarSeguirAlLider()
    {
        if (listaAliados.Count > 0)
        {
            GameObject lider = listaAliados[0];
            foreach (GameObject Aliado in listaAliados)
            {
                if (Aliado != lider)
                {
                    Aliado comp = Aliado.GetComponent<Aliado>();
                    if (comp != null)
                        comp.MoverADestino(lider.transform.position);
                }
            }
            ActualizarUI("Todos siguen a " + lider.name);
            audioSource.PlayOneShot(clipSeguirLider);
        }
    }

    // Ordena al Aliado actual curar a un aliado herido cercano
    public void OrdenarCurarAliado()
    {
        if (AliadoActual != null)
        {
            GameObject aliadoAcurar = null;
            float distanciaMinima = float.MaxValue;
            foreach (GameObject Aliado in listaAliados)
            {
                if (Aliado != AliadoActual)
                {
                    Aliado comp = Aliado.GetComponent<Aliado>();
                    if (comp != null && comp.salud < comp.saludMaxima)
                    {
                        float dist = Vector3.Distance(AliadoActual.transform.position, Aliado.transform.position);
                        if (dist < distanciaMinima)
                        {
                            distanciaMinima = dist;
                            aliadoAcurar = Aliado;
                        }
                    }
                }
            }
            if (aliadoAcurar != null)
            {
                Aliado compCurador = AliadoActual.GetComponent<Aliado>();
                if (compCurador != null)
                    compCurador.IniciarAnimacionCurar();
                Aliado compAliado = aliadoAcurar.GetComponent<Aliado>();
                if (compAliado != null)
                {
                    compAliado.RecibirCuracion(20); // Cantidad de curación arbitraria
                    ActualizarUI(AliadoActual.name + " cura a " + aliadoAcurar.name);
                    audioSource.PlayOneShot(clipCurarAliado);
                }
            }
            else
            {
                ActualizarUI("No hay aliados heridos cercanos");
            }
        }
    }

    // Agrega un nuevo Aliado a la escuadra instanciando el prefab
    public void AgregarAliado()
    {
        if (AliadoPrefab != null)
        {
            // Se posiciona en un punto aleatorio cerca del origen
            Vector3 pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            GameObject nuevoAliado = Instantiate(AliadoPrefab, pos, Quaternion.identity);
            listaAliados.Add(nuevoAliado);
            ActualizarUI("Se agregó Aliado: " + nuevoAliado.name);
            audioSource.PlayOneShot(clipAgregarAliado);
        }
    }

    // Quita un Aliado de la escuadra y lo destruye
    public void QuitarAliado(GameObject Aliado)
    {
        if (listaAliados.Contains(Aliado))
        {
            listaAliados.Remove(Aliado);
            ActualizarUI("Se quitó Aliado: " + Aliado.name);
            audioSource.PlayOneShot(clipQuitarAliado);
            Destroy(Aliado);
            if (AliadoActual == Aliado)
            {
                if (listaAliados.Count > 0)
                    AliadoActual = listaAliados[0];
                else
                    AliadoActual = null;
            }
        }
    }

    // Actualiza el mensaje de la UI y lo imprime en consola
    void ActualizarUI(string mensaje)
    {
        if (mensajeUI != null)
        {
            mensajeUI.text = mensaje;
        }
        Debug.Log(mensaje);
    }
}
