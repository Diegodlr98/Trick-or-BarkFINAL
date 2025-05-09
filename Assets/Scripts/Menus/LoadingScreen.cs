using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingScreen : MonoBehaviour
{

    [SerializeField]
    Image _LoadingBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        AsyncOperation loadLevel = SceneManager.LoadSceneAsync("Level1");

        while (loadLevel.isDone)
        {
            _LoadingBar.fillAmount = Mathf.Clamp01(loadLevel.progress / .01f);
            yield return null;
        }
    }
}