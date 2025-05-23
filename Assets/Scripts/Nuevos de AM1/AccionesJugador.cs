using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccionesJugador : A1_Entidad
{


    public GameObject BolaDeFuego;
    public GameObject BolaDeHielo;
    public GameObject Rayo;
    public GameObject ataqueRapido;
    public GameObject Flechazo;
    
    public float fuerzaDisparo = 500f;
    public Transform Origen;
    private bool estaMuerto= false;
    private bool modoMelee = false;
    // Agrega este campo y propiedad en tu clase AccionesJugador
    public float maxCoolDown = 0f;
    public float CoolDown
    {
        get => _coolDown;
        set
        {
            _coolDown = value;
            if (_coolDown > maxCoolDown)
                maxCoolDown = _coolDown;
            ActualizarBarraCoolDown();
        }
    }
    private float _coolDown = 0f;

    // Asume que tienes una referencia al RawImage de la barra de cooldown
    public UnityEngine.UI.RawImage barraCoolDown;

    // Llama a este método en Update y cuando cambie el CoolDown
    private void ActualizarBarraCoolDown()
    {
        if (barraCoolDown == null || maxCoolDown == 0f) return;
        float porcentaje = 1f - Mathf.Clamp01(_coolDown / maxCoolDown);
        var rt = barraCoolDown.rectTransform;
        float anchoBase = barraCoolDown.texture != null ? barraCoolDown.texture.width : rt.rect.width;
        // Reemplaza la línea de cálculo de anchoBase y el ajuste de la barra por lo siguiente:
        float anchoMaximo = 200f;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, anchoMaximo * porcentaje);
    }

    // En tu método Update, agrega:
    void CargarBarraDeCoolDown()
    {
        if (CoolDown > 0)
            CoolDown -= Time.deltaTime;
        if (CoolDown < 0)
            CoolDown = 0;

        float velocidadActual = agent.velocity.magnitude;
        anim.SetFloat("velocidad", velocidadActual);
        if (Vector3.Distance(gameObject.transform.position, Destino) < 1)
        {
            Detenerse();
        }
      
        ActualizarBarraCoolDown();
    }

    public override void Atacar(Vector3 Destino, string Nombre)
    {
        if (estaMuerto) return;
        // Joaco_ Indica q animacion se esta ejecutando
        if (CoolDown != 0) return;

        GameObject ProyectilUsado = null;
       //if nuevo agregado por damian
        if(Nombre == "BolaDeFuego")
        {
            anim.SetTrigger(modoMelee ? "melee1" : "magic1");
            if (!modoMelee)
            {
                ProyectilUsado = BolaDeFuego; 
            }
            
        }
        //if nuevo agregado por damian
        if( Nombre == "BolaDeHielo") 
        {
            anim.SetTrigger(modoMelee ? "melee2" : "magic2");
            if (!modoMelee)
            {
                ProyectilUsado = BolaDeHielo;
            }
            anim.SetFloat("velocidad", 0);
            agent.isStopped = true;
        }
        if(Nombre == "Rayo") 
        {
            ProyectilUsado = Rayo;
            anim.SetTrigger(modoMelee ? "melee3" : "magic3"); //nuevo
            anim.SetFloat("velocidad", 0);
            agent.isStopped = true;
        }
        transform.LookAt(Destino);
        Vector3 direccion = 
            (Destino - Origen.transform.position)
            .normalized;

        GameObject Ataque = Instantiate(
            ProyectilUsado, 
            Origen.transform.position, 
            Quaternion.LookRotation(direccion));

        if (Ataque.GetComponent<Proyectil>() != null)
        {
            Ataque.GetComponent<Proyectil>().Creador = gameObject;
            Ataque.GetComponent<Proyectil>().AutoDestruir = true;
        }

        Rigidbody rb = Ataque.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direccion * fuerzaDisparo);
        }


        Invoke("RegistrarCoolDown", 0.1f);
    }

    // Joaco_CoolDownTiempos
    public void RegistrarCoolDown()
    {
        float speed = 1f;
        string nombreAnimacion = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        //Debug.Log(nombreAnimacion);
        switch (nombreAnimacion)
        {
            case string n when n.Contains("01"):
                speed = 8f;
                break;
            case string n when n.Contains("02"):
                speed = 4f;
                break;
            case string n when n.Contains("03"):
                speed = 4f;
                break;
            default:
                speed = 1.0f;
                break;
        }
        //Debug.Log(animacion.GetCurrentAnimatorClipInfo(0)[0].clip.length + " | " + speed);
        CoolDown = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length / speed;
    }

    public override void Detenerse()
    {
        agent.isStopped = true;
    }
    public ParticleSystem Particulas;
    public override void IrAlDestino(Vector3 destino)
    {
        if (estaMuerto) return;
        agent.isStopped = false;
        //Debug.Log(1);
        transform.LookAt(destino);
        agent.SetDestination(destino);
        Destino = destino;
        Particulas.gameObject.transform.position = destino;
        Particulas.Play();
        //Debug.Log(2);

    }
   

    public override void Morir()
    {
        if (estaMuerto) return;
        Feedbacks.FeedbackRadialVisual(Color_Muere, 4);
        estaMuerto = true;
        anim.SetTrigger("life"); //nuevo
    }

    public override void OnCollision(Collision collider)
    {
        throw new System.NotImplementedException();
    }
    public Feedbacks Feedbacks;
    public Color Color_RecibeDano;
    public Color Color_ObtieneMonedas;
    public Color Color_FueAvistado;
    public Color Color_Muere;
    public Color Color_SeCura;

    public override void RecibirDanio(int cantidad)
    {
        Vida -= cantidad;
        Debug.Log(gameObject.name + " Recibio daño de " + cantidad + " le queda " + Vida, gameObject);
        Feedbacks.FeedbackRadialVisual(Color_RecibeDano, 1);
        if (Vida <= 0) 
        {
            Morir();
            Invoke("CargaEscenaDerrota", 3f);
        }
    }
    void CargaEscenaDerrota()
    {
        SceneManager.LoadScene("Derrota");
    }

    // + Agente: Navmeshagent
    // AtaqueElegido(string): void
    // GameManager
    // Inputs

    // Start is called before the first frame update
    void Start()
    {

    }
    public Vector3 Destino;
    // Update is called once per frame
    void Update()
    {

        CargarBarraDeCoolDown();

     //  if (CoolDown > 0)
         //   CoolDown -= Time.deltaTime;
       // if (CoolDown < 0)
           // CoolDown = 0;

        float velocidadActual = agent.velocity.magnitude;
        anim.SetFloat("velocidad", velocidadActual);
        if (Vector3.Distance(gameObject.transform.position, Destino) < 1)
        {
            Detenerse();
           
        }
        //if nuevo
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            modoMelee = !modoMelee; // alterna entre melee y rango

            if (modoMelee)
            {
                Debug.Log("Modo cambiado a MELEE");
                anim.SetLayerWeight(0, 0f); // capa 0 = Rango
                anim.SetLayerWeight(1, 1f); // capa 1 = Melee
                
            }
            else
            {
                Debug.Log("Modo cambiado a rango");
                anim.SetLayerWeight(0, 1f); // capa 0 = Rango
                anim.SetLayerWeight(1, 0f); // capa 1 = Melee

            }
            //fin el if nuevo
        }
    }

    public override void Colisiono(GameObject col, string TipoDeColision)
    {
        /*
        Debug.Log(
            "El _" 
            + Colision.name  
            + "_ Colisiona con _" 
            + gameObject.name 
            + "_ Con _" 
            + TipoDeColision 
            + "_ Tipo de colision"
            , gameObject);
        */
        // El _Enemigo_ Colisiona con _Jugador v2_ Con _TriggerStay_ Tipo de colision
        A3_Interactuable interactivo = col.GetComponent<A3_Interactuable>();
        if (interactivo != null)
        {
            interactivo.Interactuar();
        }
        Debug.DrawLine(col.transform.position,gameObject.transform.position);
    }
}
