using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -5);
    public float sensitivity = 5f;
    public float distance = 5f;
    public float minY = -35f;
    public float maxY = 60f;
    public LayerMask collisionMask;

    private float yaw = 0f;
    private float pitch = 0f;
    private float currentDistance; // ahora guardamos la distancia actual interpolada
    private float desiredDistance; // distancia que queremos alcanzar

    public float smoothSpeed = 10f; // velocidad de suavizado

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentDistance = distance;
        desiredDistance = distance;
    }

    void LateUpdate()
    {
        Vector2 mouseInput = Mouse.current.delta.ReadValue() * sensitivity * Time.deltaTime;
        yaw += mouseInput.x;
        pitch -= mouseInput.y;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 targetPosition = target.position + Vector3.up * offset.y;
        Vector3 direction = rotation * Vector3.back;

        RaycastHit hit;
        float sphereRadius = 0.3f;

        desiredDistance = distance; // Reseteamos
        if (Physics.SphereCast(targetPosition, sphereRadius, direction, out hit, distance, collisionMask))
        {
            desiredDistance = hit.distance - 0.2f;
            if (desiredDistance < 0.5f) desiredDistance = 0.5f; // Evitar que esté demasiado cerca
        }

        // Interpolamos suavemente
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * smoothSpeed);

        transform.position = targetPosition + direction * currentDistance;
        transform.LookAt(targetPosition + Vector3.up * 0.5f);
    }
}
