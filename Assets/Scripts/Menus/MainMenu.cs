using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ElegirEscena(string nombreNivel) //Esto es un metodo que se usara en todos los botones ya que asi podremos reutilizarlo todas las veces que necesite.
    {
        SceneManager.LoadScene(nombreNivel); //Se le agrega el string para que desde el editor de unity pueda poner el nombre de la escena que quiero.
        Time.timeScale = 1;
    }

    public void Salir() //Este metodo sera usado para que el boton salir te haga salir del juego.
    {
        Application.Quit(); //Esto ara que salgas.
    }
}
