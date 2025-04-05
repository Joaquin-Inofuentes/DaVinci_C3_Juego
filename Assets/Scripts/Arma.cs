using UnityEngine;

[System.Serializable]
public class Arma
{
    public string id;
    public string nombre;
    public string tipo;
    public string clase;
    public Texture2D icono;
    public GameObject Proyectil;
    public GameObject PrefabDeArma;
    public GameObject OrigenDeDisparos;
    [TextArea] public string descripcion;

    public string municionTipo;
    public int capacidadCartucho;
    public int balasEnCartucho;
    public int municionTotal;
    public float dañoBase;

    public float cadencia;

    public AudioClip sonidoDisparo;
    public AudioClip sonidoRecarga;
    public AudioClip sonidoCargadorVacio;

    public float SOLOMELEE_alcanceMelee;
    public float SOLOMELEE_dañoCriticoMelee;
    public float tiempoRecarga;
    public float velocidadBala;

    public Jugador DueñoDelArma;

    public void TirarDelGatillo()
    {
        Debug.Log("Se disparo");
        if (balasEnCartucho <= 0)
        {
            DueñoDelArma.Recargar();
            AudioSource.PlayClipAtPoint(sonidoCargadorVacio, DueñoDelArma.transform.position);
            return;
        }

        CrearBala();
        if (sonidoDisparo)
        {
            AudioSource.PlayClipAtPoint(sonidoDisparo, DueñoDelArma.transform.position);
        }
    }

    void CrearBala()
    {
        if (Proyectil == null || OrigenDeDisparos == null)
        {
            Debug.Log("Falta asociar los gameobjects de proyectil o Origen del disparo");
            return;
        }
        balasEnCartucho--;
        GameObject bala = Object.Instantiate(Proyectil, OrigenDeDisparos.transform.position, Quaternion.LookRotation(OrigenDeDisparos.transform.forward));
        Rigidbody rb = bala.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.velocity = OrigenDeDisparos.transform.forward * velocidadBala;
        }
    }

    public void Recargar()
    {
        int balasNecesarias = capacidadCartucho - balasEnCartucho;
        int balasARecargar = Mathf.Min(balasNecesarias, municionTotal);
        balasEnCartucho += balasARecargar;
        municionTotal -= balasARecargar;
    }
}
