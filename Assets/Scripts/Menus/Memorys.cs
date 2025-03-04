using UnityEngine;

public class Memory : MonoBehaviour
{
    public Sprite memorySprite; // Sprite específico para este Memory

    [HideInInspector]
    public SpriteRenderer memorySpriteRenderer; // Será asignado desde PlayerMovement
}
