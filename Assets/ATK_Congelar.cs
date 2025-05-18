using UnityEngine;

public class ATK_Congelar : MonoBehaviour
{
    public float timer = 3f;

    public Animator anim;
    public Transform padre;
    public Vector3 posOriginal;
    public Quaternion rotOriginal;
    public float timerActual;

    void Start()
    {
        if (anim != null) anim.speed = 0;

        posOriginal = padre.position;
        rotOriginal = padre.rotation;
        timerActual = timer;
    }

    void Update()
    {
        if (padre == null) return;

        // Mantener posición y rotación
        padre.position = posOriginal;
        padre.rotation = rotOriginal;

        // Contador
        timerActual -= Time.deltaTime;
        if (timerActual <= 0)
        {
            if (anim != null) anim.speed = 1;

            Destroy(gameObject); // eliminar congelador
        }
    }
}
