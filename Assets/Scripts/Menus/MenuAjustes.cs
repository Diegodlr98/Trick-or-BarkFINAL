using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class MenuAjustes : MonoBehaviour
{
    public GameObject ObjetoMenuAjustes;    
    public bool Ajustes = false;    
    public AudioMixer audioMixer;
    
    public Slider sliderEfectos;    
    public Slider sliderVolumen;

    public TMP_InputField inputVolumen;
    public TMP_InputField inputEfectos;
    

    void Start()
    {       

        float volumenGuardado = PlayerPrefs.GetFloat("volumen", 0.5f);
        if (volumenGuardado < 0.0001f) volumenGuardado = 0.5f;
        sliderVolumen.value = volumenGuardado;
        CambiarVolumen(volumenGuardado);

        float volumenSFX = PlayerPrefs.GetFloat("volumenSFX", 0.5f);
        if (volumenSFX < 0.0001f) volumenSFX = 0.5f;
        sliderEfectos.value = volumenSFX;
        CambiarVolumenEfectos(volumenSFX);       
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && ObjetoMenuAjustes == false)
        {
            ObjetoMenuAjustes.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && ObjetoMenuAjustes != false)
        {
            ObjetoMenuAjustes.SetActive(false);
        }
    }

    public void CambiarVolumen(float valor)
    {        
        float volumenDB = Mathf.Log10(Mathf.Clamp(valor, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MasterVolume", volumenDB);

        if (inputVolumen != null)
        {
            inputVolumen.text = valor.ToString("F2");            
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

    public void MostrarAjustes()
    {        
        ObjetoMenuAjustes.SetActive(true);        
    }

    public void VolverDeAjustes()
    {
        ObjetoMenuAjustes.SetActive(false);        
    }     
              
}
