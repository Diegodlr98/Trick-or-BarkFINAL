using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MemoryManager : MonoBehaviour
{
    public static MemoryManager Instance;

    public List<VideoClip> memoryClips; // Asigna los clips en orden desde el inspector
    private int currentMemoryIndex = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public VideoClip GetNextMemoryClip()
    {
        if (currentMemoryIndex < memoryClips.Count)
        {
            return memoryClips[currentMemoryIndex++];
        }
        return null;
    }

    public bool AllMemoriesCollected()
    {
        return currentMemoryIndex >= memoryClips.Count;
    }

    public int GetCurrentMemoryCount()
    {
        return currentMemoryIndex;
    }
}
