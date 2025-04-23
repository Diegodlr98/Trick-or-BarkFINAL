using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuPausa : MonoBehaviour
{
    public GameObject ObjetoMenuAjustes;

    public GameObject ObjetoMenuPausa;
    public bool pausa = false;

    public Slider sliderDistanciaCamara;
    public ThirdPersonCamera camara;
    public TMP_InputField inputDistanciaCamara;
    public Slider sliderSensibilidad;
    public TMP_InputField inputSensibilidad;

    void Start()
    {
        float distanciaActual = camara.distance;
        sliderDistanciaCamara.value = distanciaActual;
        inputDistanciaCamara.text = distanciaActual.ToString("F1");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausa)
            {
                ObjetoMenuPausa.SetActive(true);
                pausa = true;

                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                // Si estamos en pausa (con o sin ajustes), cerrar todo
                CerrarTodo();
            }
        }
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

        float distanciaActual = camara.distance;
        sliderDistanciaCamara.value = distanciaActual;
        inputDistanciaCamara.text = distanciaActual.ToString("F1");

        float sensibilidadActual = camara.sensitivity;
        sliderSensibilidad.value = sensibilidadActual;
        inputSensibilidad.text = sensibilidadActual.ToString("F1");
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
            {
                inputDistanciaCamara.text = nuevaDistancia.ToString("F1"); // 1 decimal
            }
        }
    }
    public void InputCambiarDistanciaCamara(string texto)
    {
        if (float.TryParse(texto, out float valor))
        {
            // Clamp dentro del rango permitido del slider
            valor = Mathf.Clamp(valor, sliderDistanciaCamara.minValue, sliderDistanciaCamara.maxValue);

            sliderDistanciaCamara.value = valor; // Esto también llamará a CambiarDistanciaCamara automáticamente si está vinculado
        }
    }
    public void CambiarSensibilidad(float nuevaSensibilidad)
    {
        if (camara != null)
        {
            camara.sensitivity = nuevaSensibilidad;

            if (inputSensibilidad != null)
            {
                inputSensibilidad.text = nuevaSensibilidad.ToString("F1");
            }
        }
    }

    public void InputCambiarSensibilidad(string texto)
    {
        if (float.TryParse(texto, out float valor))
        {
            valor = Mathf.Clamp(valor, sliderSensibilidad.minValue, sliderSensibilidad.maxValue);
            sliderSensibilidad.value = valor; // Esto llama a CambiarSensibilidad automáticamente
        }
    }




    public void Resumir()
    {
        ObjetoMenuPausa.SetActive(false);
        pausa = false;

        Time.timeScale = 1;
        Cursor.visible=true;
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
