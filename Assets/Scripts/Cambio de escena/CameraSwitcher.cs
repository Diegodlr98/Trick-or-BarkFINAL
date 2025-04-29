using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSwitcher : MonoBehaviour
{
    public Camera cameraA;
    public Camera cameraB;

    private void Start()
    {
        int lastUsed = PlayerPrefs.GetInt("LastCameraUsed", 0);

        if (lastUsed == 0)
        {
            ActivateCamera(cameraB, cameraA);
            PlayerPrefs.SetInt("LastCameraUsed", 1);
        }
        else
        {
            ActivateCamera(cameraA, cameraB);
            PlayerPrefs.SetInt("LastCameraUsed", 0);
        }

        PlayerPrefs.Save();
    }

    void ActivateCamera(Camera toActivate, Camera toDeactivate)
    {
        if (toActivate != null) toActivate.gameObject.SetActive(true);
        if (toDeactivate != null) toDeactivate.gameObject.SetActive(false);
    }
}