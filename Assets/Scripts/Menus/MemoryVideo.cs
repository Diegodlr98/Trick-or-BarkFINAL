using System.Buffers;
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

        // Pausar música
        if (playerMovement != null && playerMovement.backgroundMusic != null && playerMovement.backgroundMusic.isPlaying)
        {
            playerMovement.backgroundMusic.Pause();
        }

        if (skipMessage != null)
            skipMessage.SetActive(true);

        player = memory.GetComponent<VideoPlayer>();

        //  Usa el siguiente clip en orden
        if (player != null && MemoryManager.Instance != null)
        {
            player.clip = MemoryManager.Instance.GetNextMemoryClip();
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

        // Reanudar música
        if (playerMovement != null && playerMovement.backgroundMusic != null)
        {
            playerMovement.backgroundMusic.UnPause();
        }

        // Verificar si se han completado todas las memorias
        if (playerMovement != null && MemoryManager.Instance.AllMemoriesCollected())
        {
            Time.timeScale = 1f;
            yield return playerMovement.StartCoroutine(playerMovement.PlayMemoryVideo());
        }
        else
        {
            Time.timeScale = 1;
        }

        OnMemoryVideoComplete?.Invoke();
    }

}
