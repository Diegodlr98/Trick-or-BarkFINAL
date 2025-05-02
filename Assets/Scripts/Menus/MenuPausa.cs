using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class MenuPausa : MonoBehaviour
{
    public GameObject ObjetoMenuAjustes;
    public GameObject ObjetoMenuPausa;
    public bool pausa = false;
    public ThirdPersonCamera camara;
    public AudioMixer audioMixer;

    public Slider sliderSensibilidad;
    public Slider sliderEfectos;
    public Slider sliderDistanciaCamara;
    public Slider sliderVolumen;

    public TMP_InputField inputVolumen;
    public TMP_InputField inputEfectos;
    public TMP_InputField inputSensibilidad;
    public TMP_InputField inputDistanciaCamara;

    void Start()
    {
        // BORRAR PREFERENCIAS para testeo (descomenta si quieres hacer un reset)
         PlayerPrefs.DeleteAll(); 
         PlayerPrefs.Save();

        float volumenGuardado = PlayerPrefs.GetFloat("volumen", 0.5f);
        if (volumenGuardado < 0.0001f) volumenGuardado = 0.5f;
        sliderVolumen.value = volumenGuardado;
        CambiarVolumen(volumenGuardado);

        float volumenSFX = PlayerPrefs.GetFloat("volumenSFX", 0.5f);
        if (volumenSFX < 0.0001f) volumenSFX = 0.5f;
        sliderEfectos.value = volumenSFX;
        CambiarVolumenEfectos(volumenSFX);

        float distanciaCamara = PlayerPrefs.GetFloat("distanciaCamara", 5f);
        camara.distance = distanciaCamara;
        sliderDistanciaCamara.value = distanciaCamara;
        inputDistanciaCamara.text = distanciaCamara.ToString("F1");

        float sensibilidad = PlayerPrefs.GetFloat("sensibilidad", 2f);
        camara.sensitivity = sensibilidad;
        sliderSensibilidad.value = sensibilidad;
        inputSensibilidad.text = sensibilidad.ToString("F1");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausa)
            {
                ShowPauseMenu();
            }
            else
            {
                CerrarTodo();
            }
        }
    }

    public void CambiarVolumen(float valor)
    {
        Debug.Log("Slider volumen cambiado: " + valor);

        float volumenDB = Mathf.Log10(Mathf.Clamp(valor, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MasterVolume", volumenDB);

        if (inputVolumen != null)
        {
            inputVolumen.text = valor.ToString("F2");
            Debug.Log("Texto actualizado a: " + inputVolumen.text);
        }

        PlayerPrefs.SetFloat("volumen", valor);
        PlayerPrefs.Save();
    }

    public void CambiarVolumenEfectos(float valor)
    {
        float volumenDB = Mathf.Log10(Mathf.Clamp(valor, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("SFXVolume", volumenDB);

        if (inputEfectos != null)
        {
            inputEfectos.text = valor.ToString("F3");
        }

        PlayerPrefs.SetFloat("volumenSFX", valor);
        PlayerPrefs.Save();
    }

    private void CerrarTodo()
    {
        ObjetoMenuPausa.SetActive(false);
        ObjetoMenuAjustes.SetActive(false);
        pausa = false;

        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MostrarAjustes()
    {
        ObjetoMenuPausa.SetActive(false);
        ObjetoMenuAjustes.SetActive(true);

        sliderDistanciaCamara.value = camara.distance;
        inputDistanciaCamara.text = camara.distance.ToString("F1");

        sliderSensibilidad.value = camara.sensitivity;
        inputSensibilidad.text = camara.sensitivity.ToString("F1");
    }

    public void VolverDeAjustes()
    {
        ObjetoMenuAjustes.SetActive(false);
        ObjetoMenuPausa.SetActive(true);
    }

    public void CambiarDistanciaCamara(float nuevaDistancia)
    {
        if (camara != null)
        {
            camara.distance = nuevaDistancia;
            if (inputDistanciaCamara != null)
                inputDistanciaCamara.text = nuevaDistancia.ToString("F1");

            PlayerPrefs.SetFloat("distanciaCamara", nuevaDistancia);
            PlayerPrefs.Save();
        }
    }

    public void InputCambiarDistanciaCamara(string texto)
    {
        if (float.TryParse(texto, out float valor))
        {
            valor = Mathf.Clamp(valor, sliderDistanciaCamara.minValue, sliderDistanciaCamara.maxValue);
            sliderDistanciaCamara.value = valor;
        }
    }

    public void CambiarSensibilidad(float nuevaSensibilidad)
    {
        if (camara != null)
        {
            camara.sensitivity = nuevaSensibilidad;
            if (inputSensibilidad != null)
                inputSensibilidad.text = nuevaSensibilidad.ToString("F1");

            PlayerPrefs.SetFloat("sensibilidad", nuevaSensibilidad);
            PlayerPrefs.Save();
        }
    }

    public void InputCambiarSensibilidad(string texto)
    {
        if (float.TryParse(texto, out float valor))
        {
            valor = Mathf.Clamp(valor, sliderSensibilidad.minValue, sliderSensibilidad.maxValue);
            sliderSensibilidad.value = valor;
        }
    }

    public void Resumir()
    {
        ObjetoMenuPausa.SetActive(false);
        pausa = false;

        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowPauseMenu()
    {
        ObjetoMenuPausa.SetActive(true);
        pausa = true;

        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
