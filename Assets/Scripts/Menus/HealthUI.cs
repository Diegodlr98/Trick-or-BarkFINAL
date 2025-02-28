using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{
    public Image heartPrefab;  // Prefab del corazón
    public List<Image> hearts = new List<Image>();  // Lista de imágenes de corazones

    public void SetMaxHearts(int maxHearts)
    {
        // Eliminar corazones antiguos
        foreach (Image heart in hearts)
        {
            Destroy(heart.gameObject);
        }
        hearts.Clear();

        // Crear nuevos corazones
        for (int i = 0; i < maxHearts; i++)
        {
            Image newHeart = Instantiate(heartPrefab, transform);
            hearts.Add(newHeart);
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].gameObject.SetActive(i < currentHealth);
        }
    }
}
