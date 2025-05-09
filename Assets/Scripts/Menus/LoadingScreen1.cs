using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingScreen1 : MonoBehaviour
{

    [SerializeField]
    Image _LoadingBarMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LoadNextMenu());
    }

    IEnumerator LoadNextMenu()
    {
        AsyncOperation loadLevel = SceneManager.LoadSceneAsync("MainMenuFinal");

        while (loadLevel.isDone)
        {
            _LoadingBarMenu.fillAmount = Mathf.Clamp01(loadLevel.progress / .01f);
            yield return null;
        }
    }
}