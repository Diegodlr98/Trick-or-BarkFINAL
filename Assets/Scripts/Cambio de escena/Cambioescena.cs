using UnityEngine;
using UnityEngine.SceneManagement;
public class Cambioescena : MonoBehaviour
{
    // Aqui se pondra el nombre de la escena 
    [SerializeField] private string sceneToLoad;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Esto Verifica si el objeto con el que colisiona tiene la etiqueta obstacle
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene(sceneToLoad); // Cambia la escena
        }
    }
}
