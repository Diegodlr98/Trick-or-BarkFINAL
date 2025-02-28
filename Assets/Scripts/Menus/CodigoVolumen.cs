using UnityEngine;
using UnityEngine.UI;

public class CodigoVolumen : MonoBehaviour
{
    public Slider sliderMusica; //Para referenciar desde el editor de unity el slider de musica.
    public Slider sliderEfectos; //Para referenciar desde el editor de unity el slider de efectos.

    public float volumenMusica; //Variable para almacenar el valor del volumen de la musica
    public float volumenEfectos; //Variable para almacenar el valor del volumen de los efectos.

    public AudioSource[] musicaAudioSources; // Array para las fuentes de audio de musica
    public AudioSource[] efectosAudioSources; // Array para las fuentes de audio de efectos
    void Start()
    {
        // Cargar los volúmenes guardados en PlayerPrefs
        volumenMusica = PlayerPrefs.GetFloat("volumenMusica", 0.5f);
        volumenEfectos = PlayerPrefs.GetFloat("volumenEfectos", 0.5f);

        Debug.Log("Volumen Musica Cargado: " + volumenMusica);
        Debug.Log("Volumen Efectos Cargado: " + volumenEfectos);

        // Establecer el valor de los sliders de acuerdo al volumen guardado
        sliderMusica.value = volumenMusica;
        sliderEfectos.value = volumenEfectos;

        // Ajustar los volúmenes de las fuentes de audio de música
        foreach (var audioSource in musicaAudioSources)
        {
            audioSource.volume = volumenMusica;
        }

        // Ajustar el volumen de todos los AudioSource de efectos
        foreach (var audioSource in efectosAudioSources)
        {
            audioSource.volume = volumenEfectos;
        }
    }

    // Método para cambiar el volumen de la música
    public void ChangeSliderMusica(float valor)
    {
        volumenMusica = valor;
        PlayerPrefs.SetFloat("volumenMusica", volumenMusica); // Guardar volumen de música
        PlayerPrefs.Save();  // Asegurarse de que los cambios se guardan inmediatamente
        Debug.Log("Nuevo Volumen Musica Guardado: " + volumenMusica);

        // Ajustar el volumen de todas las fuentes de audio de música
        foreach (var audioSource in musicaAudioSources)
        {
            audioSource.volume = volumenMusica;
        }
    }

    // Método para cambiar el volumen de los efectos
    public void ChangeSliderEfectos(float valor)
    {
        volumenEfectos = valor;
        PlayerPrefs.SetFloat("volumenEfectos", volumenEfectos); // Guardar volumen de efectos
        PlayerPrefs.Save();  // Asegurarse de que los cambios se guardan inmediatamente
        Debug.Log("Nuevo Volumen Efectos Guardado: " + volumenEfectos);

        // Ajustar el volumen de todos los efectos de sonido
        foreach (var audioSource in efectosAudioSources)
        {
            audioSource.volume = volumenEfectos;
        }
    }
}
