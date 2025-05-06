using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
/// <summary>
/// Acciones del jugador
/// Define los feedbacks y resultados de cada accion del jugador
/// Animaciones, Sonidos y Graficas
/// </summary>
public class A_Jugador : MonoBehaviour
{
    public DATA_Jugador jugador;

    public AudioClip sonidoPatada;

    // Referencias para instanciar objetos
    public Transform manoJugador;
    public Camera camaraJugador;

    public void Update()
    {
        AnimacionDeMuerto();
        TimerDeRecarga();
        if (Input.GetKeyDown(KeyCode.F4))
        {
            GameManager.COMP.ReiniciarEscena();
        }
    }


    // === DISPARAR ===
    public void Disparar()
    {
        Arma arma = jugador.armaEquipada;
        if (arma == null
            ||
                (
                arma.CapacidadDelCargador <= 0
                && arma.UsaCargadores
                )
            )
        {
            if (arma.sonidoCargadorVacio)
            {
                AudioSource.PlayClipAtPoint(arma.sonidoCargadorVacio, transform.position);
            }
            return;
        }
        arma.TirarDelGatillo();
        if (jugador.GetComponent<Animation>())
        {
            jugador.GetComponent<Animation>().Play(arma.sonidoDisparo.name);
            AudioSource.PlayClipAtPoint(arma.sonidoDisparo, transform.position);
        }
    }

    private bool _estaRecargando = false;
    private float _timerRecarga = 0f; // Se toma de la clase Arma


    public void IniciarRecarga()
    {
        Arma arma = jugador.armaEquipada;

        if (arma == null
            || jugador.ObtenerMunicionEnCargador(jugador.armaEquipada.TipoDeMunicion) <= 0
            || arma.CapacidadDelCargador == arma.Consumo
            || !arma.UsaCargadores)
        {
            Debug.Log("No tenes municiones para " + jugador.armaEquipada.TipoDeMunicion);
            return;
        }

        AudioSource.PlayClipAtPoint(arma.sonidoRecarga, transform.position);
        _timerRecarga = arma.tiempoRecarga;
        _estaRecargando = true;
    }

    void TimerDeRecarga()
    {
        if (_estaRecargando)
        {
            _timerRecarga -= Time.deltaTime;
            if (_timerRecarga <= 0f)
            {
                _estaRecargando = false;
                jugador.armaEquipada.Recargar();
            }
        }
    }



    // === CURARSE ===
    public IEnumerator Curarse(Equipamiento kitMedico)
    {
        if (kitMedico.usosActuales <= 0)
        {
            AudioSource.PlayClipAtPoint(kitMedico.sonidoFallido, transform.position);
            yield break;
        }

        jugador.GetComponent<Animation>().Play(kitMedico.animacionUso.name);
        AudioSource.PlayClipAtPoint(kitMedico.sonidoActivacion, transform.position);

        yield return new WaitForSeconds(kitMedico.tiempoUso);

        jugador.Curarse(kitMedico.saludRestaurada);
        kitMedico.usosActuales--;
    }

    // === MORIR ===
    public RawImage pantallaMuerte;
    public TextMeshProUGUI textoMuerte;
    public float tiempoFade = 2f; // Duración de la animación

    private float timerMuerte = 0f;
    private bool activandoMuerte = false;

    void AnimacionDeMuerto()
    {
        if (activandoMuerte)
        {
            pantallaMuerte.gameObject.SetActive(true);
            timerMuerte += Time.deltaTime;
            float alpha = Mathf.Clamp01(timerMuerte / tiempoFade);

            pantallaMuerte.color = new Color(0, 0, 0, alpha);
            textoMuerte.color = new Color(textoMuerte.color.r, textoMuerte.color.g, textoMuerte.color.b, alpha);

            if (timerMuerte >= tiempoFade)
            {
                GameManager.COMP.ReiniciarEscena();
                pantallaMuerte.gameObject.SetActive(false);
                activandoMuerte = false;
            }
        }
    }

    public void Morir()
    {
        jugador.estado = "muerto";

        if (jugador.GetComponent<Animation>())
        {
            jugador.GetComponent<Animation>().Play("animacionMorir");
        }

        activandoMuerte = true;
        timerMuerte = 0f;

        // Desactivar controles, mostrar UI muerte
    }


    // === REVIVIR ===
    public void Revivir(Vector3 posicionRespawn)
    {
        jugador.estado = "vivo";
        jugador.salud = jugador.saludMaxima;
        jugador.transform.position = posicionRespawn;
        jugador.GetComponent<Animation>().Play("animacionRevivir");
        // Activar controles, ocultar UI muerte
    }

    // === CUERPO A CUERPO ===
    public void AtaqueMelee()
    {
        Arma arma = jugador.armaEquipada;

        AudioSource.PlayClipAtPoint(arma.sonidoDisparo, transform.position);

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, arma.SOLOMELEE_alcanceMelee, transform.forward, arma.SOLOMELEE_alcanceMelee);
        foreach (RaycastHit hit in hits)
        {
            Enemigo_Viejo enemigo = hit.collider.GetComponent<Enemigo_Viejo>();
            if (enemigo != null)
            {
                enemigo.RecibirDanio(arma.dañoBase, transform);
            }
        }
    }


    // === DAR PATADA ===
    public void DarPatada()
    {
        jugador.GetComponent<Animation>().Play("animacionPatada");
        AudioSource.PlayClipAtPoint(jugador.sonidoPatada, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(transform.forward * 500f);
        }
    }



    // === CAMBIAR DE ARMA ===
    public void CambiarDeArma(Arma nuevaArma)
    {
        if (jugador.armaEquipada == null) return;

        Arma nuevaArmaPendiente = nuevaArma;
        jugador.armaEquipada = nuevaArmaPendiente;
        //jugador.GetComponent<Animation>().Play(nuevaArmaPendiente.animacionInspeccion.name);
        Instantiate(nuevaArmaPendiente.PrefabDeArma, manoJugador);
    }


    // === LANZAR GRANADA ===
    public void LanzarGranada(Equipamiento granada)
    {
        if (granada.usosActuales <= 0)
        {
            AudioSource.PlayClipAtPoint(granada.sonidoFallido, transform.position);
            return;
        }

        jugador.GetComponent<Animation>().Play(granada.animacionUso.name);
        AudioSource.PlayClipAtPoint(granada.sonidoActivacion, transform.position);

        GameObject granadaObj = Instantiate(granada.prefabUso, manoJugador.position, Quaternion.identity);
        Rigidbody rb = granadaObj.GetComponent<Rigidbody>();
        rb.AddForce(camaraJugador.transform.forward * 15f, ForceMode.VelocityChange);

        granada.usosActuales--;
    }

    // === MARCAR ENEMIGO ===
    public void MarcarEnemigo()
    {
        RaycastHit hit;
        if (Physics.Raycast(camaraJugador.transform.position, camaraJugador.transform.forward, out hit, 100f))
        {
            if (hit.collider.CompareTag("Enemigo"))
            {
                // Mostrar marcador visual
                Debug.Log("Enemigo marcado: " + hit.collider.name);
            }
        }
    }

    // === TOMAR MUNICIÓN ===
    public void TomarMunicion(Equipamiento municion)
    {
        jugador.inventarioMunicion[municion.tipo] += municion.usosActuales;
        AudioSource.PlayClipAtPoint(municion.sonidoRecoger, transform.position);
        Destroy(municion.prefabMundo);
    }

    // === TOMAR KIT MÉDICO ===
    public void TomarKitMedico(Equipamiento kitMedico)
    {
        jugador.inventarioEquipamiento.Add(kitMedico);
        AudioSource.PlayClipAtPoint(kitMedico.sonidoRecoger, transform.position);
        Destroy(kitMedico.prefabMundo);
    }

    // === CURARSE EN KIT MÉDICO ===
    public IEnumerator CurarseEnKitMedico(Equipamiento kitMedico)
    {
        if (kitMedico.usosActuales <= 0)
        {
            AudioSource.PlayClipAtPoint(kitMedico.sonidoFallido, transform.position);
            yield break;
        }

        jugador.GetComponent<Animation>().Play(kitMedico.animacionUso.name);
        AudioSource.PlayClipAtPoint(kitMedico.sonidoActivacion, transform.position);

        yield return new WaitForSeconds(kitMedico.tiempoUso);

        jugador.Curarse(kitMedico.saludRestaurada);
        kitMedico.usosActuales--;
    }
}


