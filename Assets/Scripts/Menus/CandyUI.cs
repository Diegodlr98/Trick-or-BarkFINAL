using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CandyUI : MonoBehaviour
{
    public TextMeshProUGUI candyText;
    

    public void UpdateCandyCount(int candyCount)
    {
        candyText.text =  candyCount.ToString();
    }
   
}

