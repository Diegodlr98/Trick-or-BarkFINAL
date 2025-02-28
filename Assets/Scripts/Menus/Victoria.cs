using UnityEngine;
using UnityEngine.SceneManagement;

public class Victoria : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Esto lo que va hacer es que pillara la escena en la que estamos ahora, el index de la escena cambiara a la siguiente.
            
            // Para desbloquear el cursor y hacerlo visible:
            Cursor.lockState = CursorLockMode.None;  // Desbloquea el cursor
            Cursor.visible = true;  // Hace visible el cursor
        }
    }

}
