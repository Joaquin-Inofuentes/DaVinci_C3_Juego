using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Jugador : MonoBehaviour
{
    // === ATRIBUTOS DEL JUGADOR ===
    public float salud = 100f;
    public float saludMaxima = 100f;

    // === INVENTARIO DE ARMAS ===
    public Arma armaEquipada;
    public List<Arma> armasDisponibles = new List<Arma>();
    public List<Texture2D> iconosArmas = new List<Texture2D>();

    // === INVENTARIO DE EQUIPAMIENTOS ===
    public Equipamiento equipamientoActivo;
    public List<Equipamiento> equipamientosDisponibles = new List<Equipamiento>();
    public List<Texture2D> iconosEquipamientos = new List<Texture2D>();

    // === CURACIÓN AUTOMÁTICA ===
    private float tiempoDesdeUltimoDanio = 0f;
    private float tiempoParaCuracionAutomatica = 15f;
    private float velocidadCuracionAutomatica = 5f; // HP por segundo

    // === TIMERS ===
    private float timerRecarga = 0f;
    private float timerUsoEquipamiento = 0f;

    private AccionesJugador accionesJugador;
    public AudioClip sonidoPatada;

    // Diccionario donde la clave es el tipo de munición (string)
    // y el valor es la cantidad (int)
    public Dictionary<string, int> inventarioMunicion = new Dictionary<string, int>();
    public List<Equipamiento> inventarioEquipamiento = new List<Equipamiento>();
    public string estado;

    void Start()
    {
        accionesJugador = GetComponent<AccionesJugador>();

        if (armasDisponibles.Count > 0)
            EquiparArma(armasDisponibles[0]);

        if (equipamientosDisponibles.Count > 0)
            equipamientoActivo = equipamientosDisponibles[0];
    }

    void Update()
    {
        ManejarTimers();
        ManejarInputs();
        ManejarCuracionAutomatica();
        EfectoDeTomaDano();
    }

    // === MANEJO DE TIMERS ===
    void ManejarTimers()
    {
        if (timerRecarga > 0) timerRecarga -= Time.deltaTime;
        if (timerUsoEquipamiento > 0) timerUsoEquipamiento -= Time.deltaTime;

        tiempoDesdeUltimoDanio += Time.deltaTime;
    }

    // === MANEJO DE INPUTS ===
    void ManejarInputs()
    {
        if (Input.GetKeyDown(KeyCode.R) && timerRecarga <= 0)
            Recargar();

        for (int i = 0; i < armasDisponibles.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                CambiarArma(armasDisponibles[i]);
        }

        if (Input.GetKeyDown(KeyCode.C) && timerUsoEquipamiento <= 0 && equipamientoActivo != null && equipamientoActivo.tipo == "medico")
            UsarEquipamiento(equipamientoActivo);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            accionesJugador.Disparar();
        }
    }

    // === CURACIÓN AUTOMÁTICA ===
    void ManejarCuracionAutomatica()
    {
        if (tiempoDesdeUltimoDanio >= tiempoParaCuracionAutomatica && salud < saludMaxima)
        {
            salud += velocidadCuracionAutomatica * Time.deltaTime;
            salud = Mathf.Min(salud, saludMaxima);
        }
    }

    
    public RawImage dañoPantalla;
    public float tiempoTotal = 3f; // Duración total del efecto
    private float timer = 0f;
    private bool tomandoDanio = false;

    void EfectoDeTomaDano()
    {
        if (tomandoDanio)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(Mathf.PingPong(timer, tiempoTotal / 2) / (tiempoTotal / 2));
            dañoPantalla.color = new Color(dañoPantalla.color.r, dañoPantalla.color.g, dañoPantalla.color.b, alpha);

            if (timer >= tiempoTotal)
            {
                tomandoDanio = false;
                timer = 0f;
                dañoPantalla.color = new Color(dañoPantalla.color.r, dañoPantalla.color.g, dañoPantalla.color.b, 0);
            }
        }
    }

    public void RecibirDanio(float cantidad)
    {
        salud -= cantidad;
        tiempoDesdeUltimoDanio = 0f;
        tomandoDanio = true;
        timer = 0f;

        if (salud <= 0)
            Morir();
    }

    // === MÉTODOS DE SALUD ===
    public void Morir()
    {
        salud = 0;
        accionesJugador.Morir();
        Debug.Log("Jugador ha muerto.");
    }

    public void Revivir(Vector3 posicionRespawn)
    {
        salud = saludMaxima;
        accionesJugador.Revivir(posicionRespawn);
        Debug.Log("Jugador revivido.");
    }

    // === INVENTARIO DE ARMAS ===
    public void AgregarArma(Arma nuevaArma, Texture2D icono)
    {
        if (!armasDisponibles.Contains(nuevaArma))
        {
            armasDisponibles.Add(nuevaArma);
            iconosArmas.Add(icono);
            Debug.Log($"Arma agregada: {nuevaArma.nombre}");
        }
    }

    void CambiarArma(Arma nuevaArma)
    {
        if (nuevaArma == armaEquipada)
            return;

        accionesJugador.CambiarDeArma(nuevaArma);
        armaEquipada = nuevaArma;

        Debug.Log($"Arma equipada: {nuevaArma.nombre}");
    }

    void EquiparArma(Arma arma)
    {
        armaEquipada = arma;
        Instantiate(arma.PrefabDeArma, accionesJugador.manoJugador);
        Debug.Log($"Arma inicial equipada: {arma.nombre}");
    }

    public float tiempoRecargaFijo = 1.5f; // tiempo universal de recarga

    // === RECARGAR ===
    public void Recargar()
    {
        if (armaEquipada == null || armaEquipada.municionTotal <= 0 || armaEquipada.balasEnCartucho == armaEquipada.capacidadCartucho)
            return;

        armaEquipada.Recargar();
        //accionesJugador.jugador.GetComponent<Animation>().Play(armaEquipada.animacionRecarga.name);
        AudioSource.PlayClipAtPoint(armaEquipada.sonidoRecarga, transform.position);

        timerRecarga = tiempoRecargaFijo;
        Debug.Log($"Recargando arma: {armaEquipada.nombre}");
    }


    // === INVENTARIO DE EQUIPAMIENTOS ===
    public void AgregarEquipamiento(Equipamiento nuevoEquipamiento, Texture2D icono)
    {
        if (!equipamientosDisponibles.Contains(nuevoEquipamiento))
        {
            equipamientosDisponibles.Add(nuevoEquipamiento);
            iconosEquipamientos.Add(icono);
            Debug.Log($"Equipamiento agregado: {nuevoEquipamiento.nombre}");
        }
    }

    void UsarEquipamiento(Equipamiento equip)
    {
        if (equip.usosActuales <= 0)
        {
            AudioSource.PlayClipAtPoint(equip.sonidoFallido, transform.position);
            return;
        }

        accionesJugador.jugador.GetComponent<Animation>().Play(equip.animacionUso.name);
        AudioSource.PlayClipAtPoint(equip.sonidoActivacion, transform.position);

        Curarse(equip.saludRestaurada);
        equip.usosActuales--;

        timerUsoEquipamiento = equip.tiempoUso;
        Debug.Log($"Usado equipamiento: {equip.nombre}, salud actual: {salud}");
    }

    public void Curarse(float cantidad)
    {
        salud = Mathf.Min(salud + cantidad, saludMaxima);
    }
}
