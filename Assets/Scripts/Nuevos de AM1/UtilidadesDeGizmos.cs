using UnityEngine;

public static class UtilidadesDeGizmos
{
    // 1) Sin parámetros extra: usa radio, 32 segmentos y rojo
    public static void DibujarCirculoPlano(Vector3 centro, float radio)
    {
        DibujarCirculoPlano(centro, radio, 32, Color.red);
    }

    // 2) Sin color, pasa segmentos
    public static void DibujarCirculoPlano(Vector3 centro, float radio, int segmentos)
    {
        DibujarCirculoPlano(centro, radio, segmentos, Color.red);
    }

    // 3) Todos los parámetros: centro, radio, segmentos, color
    public static void DibujarCirculoPlano(Vector3 centro, float radio, int segmentos, Color color)
    {
        Vector3 anterior = centro + new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)) * radio;

        Gizmos.color = color;

        for (int i = 1; i <= segmentos; i++)
        {
            float angulo = i * Mathf.PI * 2 / segmentos;
            Vector3 punto = centro + new Vector3(Mathf.Cos(angulo), 0, Mathf.Sin(angulo)) * radio;
            Gizmos.DrawLine(anterior, punto);
            anterior = punto;
        }

        Gizmos.DrawLine(centro + new Vector3(-radio / 2, 0, 0), centro + new Vector3(radio / 2, 0, 0));
        Gizmos.DrawLine(centro + new Vector3(0, 0, -radio / 2), centro + new Vector3(0, 0, radio / 2));
    }

    // 4) Con color explícito
    public static void DibujarLineaConEsfera(Vector3 origen, Vector3 destino, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(origen, destino);
        Gizmos.DrawSphere(destino, 0.3f);
    }

    // 5) Sin color, usa blanco por defecto
    public static void DibujarLineaConEsfera(Vector3 origen, Vector3 destino)
    {
        DibujarLineaConEsfera(origen, destino, Color.white);
    }
}
