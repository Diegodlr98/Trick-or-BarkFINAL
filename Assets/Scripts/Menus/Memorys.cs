using UnityEngine;

public class Memory : MonoBehaviour
{
    public Sprite memorySprite; // Sprite espec�fico para este Memory

    [HideInInspector]
    public SpriteRenderer memorySpriteRenderer; // Ser� asignado desde PlayerMovement
}
