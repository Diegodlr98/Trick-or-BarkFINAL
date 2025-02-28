using UnityEngine;

public class MenuPausa : MonoBehaviour
{

    public GameObject ObjetoMenuPausa;
    public bool pausa = false;
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pausa == false)
            {
                ObjetoMenuPausa.SetActive(true);
                pausa = true;

                Time.timeScale = 0; // Esto si esta en 0 hace que se pause el nivel.
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (pausa == true)
            {
                Resumir();
            }
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
