using UnityEngine;

[System.Serializable]
public class Equipamiento
{
    // === ATRIBUTOS B�SICOS ===
    public string id;
    public string nombre;
    public string tipo;
    public Texture2D icono;
    public GameObject prefabMundo;
    public GameObject prefabUso;
    [TextArea] public string descripcion;

    // === MEC�NICAS DE USO ===
    public bool esConsumible;
    public int usosMaximos;
    public int usosActuales;
    public float tiempoUso;
    public float cooldown;
    public int stackMaximo;

    // === SONIDOS ===
    public AudioClip sonidoRecoger;
    public AudioClip sonidoActivacion;
    public AudioClip sonidoRecarga;
    public AudioClip sonidoFallido;

    // === ANIMACIONES ===
    public AnimationClip animacionUso;
    public AnimationClip animacionEquipar;

    // === PAR�METROS POR TIPO ===
    public float saludRestaurada;
    public bool curaProgresiva;
    public float blindajeOtorgado;
    public float velocidadBoost;
    public float duracionBoost;
    public float areaEfecto;
    public float da�oPorSegundo;
    public float retrasoActivacion;
    public float anguloVision;
    public string proyectilTorreta;
    public float absorcionDanio;
    public float anguloProteccion;
    public float durabilidadMaxima;

    // === M�TODOS ===
    public void Usar(Jugador jugador)
    {
        if (usosActuales <= 0)
        {
            AudioSource.PlayClipAtPoint(sonidoFallido, jugador.transform.position);
            return;
        }

        usosActuales--;
        AudioSource.PlayClipAtPoint(sonidoActivacion, jugador.transform.position);
        // Implementar l�gica espec�fica seg�n tipo
    }

    public void RecargarUsos(int cantidad)
    {
        usosActuales = Mathf.Min(usosActuales + cantidad, usosMaximos);
    }
}
