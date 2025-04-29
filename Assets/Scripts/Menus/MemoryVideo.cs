using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MemoryVideo : MonoBehaviour
{
    public GameObject memory;
    public PlayerMovement playerMovement;
    public GameObject skipMessage;

    public VideoPlayer player;
    public System.Action OnMemoryVideoComplete;

    public IEnumerator playMemory()
    {
        Time.timeScale = 0;
        memory.SetActive(true);

        if (skipMessage != null)
            skipMessage.SetActive(true);

        player = memory.GetComponent<VideoPlayer>();
        if (player != null)
        {
            player.Play();

            while (!player.isPlaying)
                yield return null;

            while (player.isPlaying)
            {
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    player.Stop();
                    break;
                }
                yield return null;
            }
        }

        if (skipMessage != null)
            skipMessage.SetActive(false);

        memory.SetActive(false);

        if (playerMovement != null && playerMovement.memoryCount >= 6)
        {
            Time.timeScale = 1f; // REACTIVAR TIEMPO ANTES DE LA CINEMÁTICA FINAL
            yield return playerMovement.StartCoroutine(playerMovement.PlayMemoryVideo());
        }
        else
        {
            Time.timeScale = 1;
        }

        OnMemoryVideoComplete?.Invoke();
    }
}
