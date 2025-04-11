using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MemoryVideo : MonoBehaviour
{
    public GameObject memory;

    public VideoPlayer player;

    public IEnumerator playMemory()
    {
        Time.timeScale = 0;
        memory.SetActive(true);
        player = memory.GetComponent<VideoPlayer>();
        if (player != null)
        {
            // Espera hasta que termine el video
            while (!player.isPlaying)
            {
                yield return null;
            }
            while (player.isPlaying)
            {
                yield return null;
            }
            
        }
        Time.timeScale = 1;
        memory.SetActive(false);
    }
}
