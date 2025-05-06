using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombosDePelea : MonoBehaviour
{
    // Propiedad p�blica
    // Start is called before the first frame update
    void Start()
    {

    }

    public bool IsMagicMode => isMagicMode;

    // Estado interno
    private bool isMagicMode;

    void Update()
    {
        Attack();
    }

    public void Attack()
    {
        if (isMagicMode)
            MagicAttack();
        else
            PhysicalAttack();
    }

    private void PhysicalAttack()
    {
        // L�gica de ataque f�sico
    }

    private void MagicAttack()
    {
        // L�gica de ataque m�gico
    }


    /*
     * 
     AccionesJugador
     bool isMagicMode

     + void Update()
+ void Attack()
- void PhysicalAttack()
- void MagicAttack()
+ Update()
+ Attack()
+ PhysicalAttack
+ MagicAttack
+ IsMagicMode : bool
     */


}
