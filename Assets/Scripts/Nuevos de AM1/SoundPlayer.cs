using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    AudioSource source;
    int loopsLeft;
    public string CurrentClipName { get; private set; }

    void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
    }

    // Llamado por AudioManager
    public void Play(AudioClip clip, float volume)
    {
        CurrentClipName = clip.name;
        source.clip = clip;
        source.volume = volume;
        source.loop = false;
        source.Play();
        Destroy(gameObject, clip.length);
    }

    // Play en loop n veces
    public void PlayLoop(AudioClip clip, int times, float volume)
    {
        CurrentClipName = clip.name;
        source.clip = clip;
        source.volume = volume;
        loopsLeft = times;
        source.loop = false;
        source.Play();
        Invoke(nameof(OnLoopEnd), clip.length);
    }

    void OnLoopEnd()
    {
        loopsLeft--;
        if (loopsLeft > 0)
        {
            source.Play();
            Invoke(nameof(OnLoopEnd), source.clip.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Métodos públicos para control
    public void Stop()
    {
        source.Stop();
        Destroy(gameObject);
    }

    public void SetVolume(float volume)
    {
        source.volume = volume;
    }
}
