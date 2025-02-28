using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryUI : MonoBehaviour
{
    public TextMeshProUGUI memoryText;

    public void UpdateMemoryCount(int memoryCount)
    {
        memoryText.text = memoryCount.ToString();
    }
}

