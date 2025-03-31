using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialMessages : MonoBehaviour
{
    
    public PlayerMovement player; // Asigna el script del jugador en el Inspector
    public TextMeshProUGUI messageText; // Asigna un Text de la UI en el Inspector

    private bool doubleJumpMessageShown = false;
    private bool dashMessageShown = false;

    private void Start()
    {
        StartCoroutine(ShowInitialMessages());
    }

    private IEnumerator ShowInitialMessages()
    {
        // Mensaje inicial
        ShowMessage("¡Usa WASD para moverte y recoge chuches para obtener poderes!.");
        yield return new WaitForSeconds(5f);
        HideMessage();

        // Mensaje programado después de un tiempo
        yield return new WaitForSeconds(3f);
        ShowMessage("Salta con ESPACIO, recoge fragmentos de memoria.");
        yield return new WaitForSeconds(5f);
        HideMessage();
    }

    private void Update()
    {
        Debug.Log("Cantidad de chuches: " + player.CandyCount);

        // Mensaje para el doble salto (se muestra solo una vez)
        if (player.CandyCount >= 15 && !doubleJumpMessageShown)
        {
            doubleJumpMessageShown = true; // Evita que se muestre repetidamente
            ShowMessage("¡Has desbloqueado el DOBLE SALTO!");
            StartCoroutine(HideAfterDelay(5f));
        }

        // Mensaje para el dash (se muestra solo una vez)
        if (player.CandyCount >= 25 && !dashMessageShown)
        {
            dashMessageShown = true; // Evita que se muestre repetidamente
            ShowMessage("¡Has desbloqueado el DASH! Usa E para impulsarte.");
            StartCoroutine(HideAfterDelay(5f));
        }
    }

    private void ShowMessage(string text)
    {
        messageText.text = text;
        messageText.gameObject.SetActive(true);
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideMessage();
    }

    private void HideMessage()
    {
        messageText.gameObject.SetActive(false);
    }
}