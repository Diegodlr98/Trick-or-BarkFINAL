using UnityEngine;
using UnityEngine.Audio;

public class AsignarGrupoAudio : MonoBehaviour
{
    public AudioMixerGroup grupoSFX;

    void Awake()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && grupoSFX != null)
        {
            audio.outputAudioMixerGroup = grupoSFX;
        }
        else
        {
            Debug.LogWarning($"[AsignarGrupoAudio] Faltan componentes en {gameObject.name}", this);
        }
    }
}
