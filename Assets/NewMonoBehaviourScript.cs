using UnityEngine;
using UnityEngine.Rendering;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GraphicsSettings.defaultRenderPipeline != null)
        {
            if (GraphicsSettings.defaultRenderPipeline.GetType().Name == "HDRenderPipelineAsset")
            {
                Debug.Log("High Definition Render Pipeline (HDRP) is being used.");
            }
            else if (GraphicsSettings.defaultRenderPipeline.GetType().Name == "UniversalRenderPipelineAsset")
            {
                Debug.Log("Universal Render Pipeline (URP) is being used.");
            }
        }
        else
        {
            Debug.Log("Using Built-in (Legacy) Render Pipeline");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
