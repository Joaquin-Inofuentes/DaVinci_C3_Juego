using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static public List<AudioClip> clips = new List<AudioClip>();
    static public AudioManager instance;

    public List<AudioClip> ClipsNostatics = new List<AudioClip>();

    // En Resources/Audio/
    // Carpeta: Assets/Resources/Audio/

    public void Start()
    {
        EscanearSonidosDisponibles();
    }

    void Update()
    {
        if (instance == null)
            instance = this;

        if(clips.Count != ClipsNostatics.Count)
            ClipsNostatics = clips;
    }

    // 1️⃣ Escanea y carga todos los clips de Resources/Audio/
    public static void EscanearSonidosDisponibles()
    {
        clips.Clear();
        AudioClip[] loaded = Resources.LoadAll<AudioClip>("Audio");
        clips.AddRange(loaded);
    }

    // 2️⃣ Reproducir sonido en posición world
    public static void ReproducirSonido(Vector3 origen, string nombre, int volumen = 100)
    {
        AudioClip clip = clips.Find(c => c.name == nombre);
        if (clip == null) return;
        GameObject go = new GameObject("Sound_" + nombre);
        go.transform.position = origen;
        var sp = go.AddComponent<SoundPlayer>();
        sp.Play(clip, volumen / 100f);
    }

    // 3️⃣ Reproducir sonido como hijo de un Transform
    public static void ReproducirSonidoConPadre(Transform padre, string nombre, int volumen = 100)
    {
        AudioClip clip = clips.Find(c => c.name == nombre);
        if (clip == null) return;
        GameObject go = new GameObject("Sound_" + nombre);
        go.transform.SetParent(padre, false);
        go.transform.localPosition = Vector3.zero;
        var sp = go.AddComponent<SoundPlayer>();
        sp.Play(clip, volumen / 100f);
    }

    // 4️⃣ Reproducir en loop n veces
    public static void ReproducirSonidoEnLoop(Vector3 origen, string nombre, int veces, int volumen = 100)
    {
        AudioClip clip = clips.Find(c => c.name == nombre);
        if (clip == null || veces <= 0) return;
        GameObject go = new GameObject("LoopSound_" + nombre);
        go.transform.position = origen;
        var sp = go.AddComponent<SoundPlayer>();
        sp.PlayLoop(clip, veces, volumen / 100f);
    }

    // 5️⃣ Detener por posición exacta
    public static void DetenerSonido(Vector3 ubicacion)
    {
        foreach (var sp in FindObjectsOfType<SoundPlayer>())
        {
            if (sp.transform.position == ubicacion)
                sp.Stop();
        }
    }

    // 6️⃣ Detener por nombre
    public static void DetenerSonido(string nombre)
    {
        foreach (var sp in FindObjectsOfType<SoundPlayer>())
        {
            if (sp.CurrentClipName == nombre)
                sp.Stop();
        }
    }
}
