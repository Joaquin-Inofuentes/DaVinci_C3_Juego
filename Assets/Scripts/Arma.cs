using UnityEngine;

[System.Serializable]
public class Arma
{
    public string id;
    public string tipo;
    public string nombre;
    //public string clase;
    public Texture2D icono;
    public GameObject Proyectil;
    public GameObject PrefabDeArma;
    public GameObject OrigenDeDisparos;
    //public string descripcion;

    public string TipoDeMunicion;
    public int Consumo; // Cantidad de balas o mana o lo q sea q consuma su cantidad
    public int CapacidadDelCargador; // Balas por cargador, Cantidad de usos maximos como granadas o flechas
    public bool UsaCargadores;
    //public int municionTotal;
    public float da�oBase;

    public float cadencia;

    public AudioClip sonidoDisparo;
    public AudioClip sonidoRecarga;
    public AudioClip sonidoCargadorVacio;

    public float SOLOMELEE_alcanceMelee;
    //public float SOLOMELEE_da�oCriticoMelee;
    public float tiempoRecarga;
    public float velocidadBala;

    public DATA_Jugador Due�oDelArma;

    public void TirarDelGatillo()
    {
        
        if (CapacidadDelCargador <= 0 && UsaCargadores)
        {
            Due�oDelArma.Recargar();
            AudioSource.PlayClipAtPoint(sonidoCargadorVacio, Due�oDelArma.transform.position);
            return;
        }
        //Debug.Log(2);
        CrearBala();
        if (sonidoDisparo)
        {
            AudioSource.PlayClipAtPoint(sonidoDisparo, Due�oDelArma.transform.position);
        }
    }



    void CrearBala()
    {
        if (Proyectil == null || OrigenDeDisparos == null)
        {
            Debug.Log("Falta asociar los gameobjects de proyectil o Origen del disparo");
            return;
        }
        if (UsaCargadores) // Si, es un arma. Consume cargadores
            CapacidadDelCargador--;
        if (!UsaCargadores)
        {
            if (Due�oDelArma.ObtenerMunicionEnCargador(TipoDeMunicion) < Consumo)
            {
                Debug.Log($"Falta {TipoDeMunicion} para hacer la accion");
                return;
            }
            Due�oDelArma.AgregarMunicion(TipoDeMunicion, -Consumo);
        }


        GameObject bala = Object.Instantiate(
        Proyectil,
        OrigenDeDisparos.transform.position,
        Quaternion.LookRotation
        (OrigenDeDisparos.transform.forward));
        bala.GetComponent<Bala>().CapaAOmitir = Due�oDelArma.gameObject.layer;
        Rigidbody rb = bala.GetComponent<Rigidbody>();
        Debug.Log("Se disparo 1 " + Proyectil.name + " Que consume " + Consumo + " de " + TipoDeMunicion);
        if (rb)
        {
            rb.velocity = OrigenDeDisparos.transform.forward * velocidadBala;
        }
    }

    public void Recargar()
    {
        if (UsaCargadores) // Si, es un arma. Consume cargadores
        {
            int balasNecesarias = Consumo - CapacidadDelCargador;
            int balasARecargar = Mathf.Min(balasNecesarias, Due�oDelArma.ObtenerMunicionEnCargador(TipoDeMunicion));
            CapacidadDelCargador += balasARecargar;
            Due�oDelArma.AgregarMunicion(TipoDeMunicion, -balasARecargar);
        }
        if (!UsaCargadores) // Si, es un hechizo. Consume mana
        {
            Debug.Log($"El {nombre} no recarga por q no usa cargadores. Usa {TipoDeMunicion}");
            //Due�oDelArma.AgregarMunicion(TipoDeMunicion, -Consumo);
        }
    }
}
