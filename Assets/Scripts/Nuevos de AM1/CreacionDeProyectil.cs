using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreacionDeProyectil : MonoBehaviour
{
    /*
     feedbacks
     + Daño : int
    OnCollisionEnter() : void
     */

    // Atributo público para daño
    public int Daño;

    // Llamado cuando ocurre una colisión
    private void OnCollisionEnter(Collision collision)
    {
        // Lógica al colisionar (por ejemplo, aplicar daño al objetivo)
        Debug.Log("Colisión con: " + collision.gameObject.name);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
