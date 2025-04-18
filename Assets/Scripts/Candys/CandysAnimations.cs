using UnityEngine;

public class CandysAnimations : MonoBehaviour
{
    public float rotationSpeed = 50f; // Velocidad de rotación en Y
    public float floatAmplitude = 0.5f; // Altura del movimiento de subida y bajada
    public float floatFrequency = 1f; // Frecuencia del movimiento de subida y bajada

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Rotación solo en el eje Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);


        // Movimiento vertical (sube y baja)
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}