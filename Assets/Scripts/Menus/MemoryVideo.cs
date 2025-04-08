using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MemoryVideo : MonoBehaviour
{
    public GameObject memory;

    public VideoPlayer player;

    private IEnumerator playMemory()
    {
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
    }
}
