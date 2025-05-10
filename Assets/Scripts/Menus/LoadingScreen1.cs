
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen1 : MonoBehaviour
{
    [SerializeField] Image _LoadingBar;

    [SerializeField] float minLoadTime = 2f; // Tiempo m�nimo de pantalla de carga (en segundos)

    void Start()
    {
        StartCoroutine(LoadNextMenu());
    }

    IEnumerator LoadNextMenu()
    {
        float fakeTimer = 0f;

        AsyncOperation loadLevel = SceneManager.LoadSceneAsync("MainMenuFinal");
        loadLevel.allowSceneActivation = false;

        while (!loadLevel.isDone)
        {
            fakeTimer += Time.deltaTime;

            // El progreso de Unity llega hasta 0.9 (el 0.1 restante es activaci�n de escena)
            float progress = Mathf.Clamp01(loadLevel.progress / 0.9f);
            _LoadingBar.fillAmount = Mathf.Clamp01((progress + (fakeTimer / minLoadTime)) / 2f);

            // Espera artificial hasta que pasen los segundos deseados y la escena est� lista
            if (loadLevel.progress >= 0.9f && fakeTimer >= minLoadTime)
            {
                _LoadingBar.fillAmount = 1f;
                yield return new WaitForSeconds(0.5f); // peque�a pausa al completar
                loadLevel.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
