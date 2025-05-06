using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreacionDeProyectil : MonoBehaviour
{
    /*
     feedbacks
     + Da�o : int
    OnCollisionEnter() : void
     */

    // Atributo p�blico para da�o
    public int Da�o;

    // Llamado cuando ocurre una colisi�n
    private void OnCollisionEnter(Collision collision)
    {
        // L�gica al colisionar (por ejemplo, aplicar da�o al objetivo)
        Debug.Log("Colisi�n con: " + collision.gameObject.name);
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
