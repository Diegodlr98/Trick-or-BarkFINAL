using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{

    public PlayerMovement playerMovement; //  Asignar desde el Inspector
    public Transform target; // El jugador
    public Vector3 offset = new Vector3(0, 2, -5);
    public float sensitivity = 5f;
    public float distance = 5f;
    public float minY = -35f;
    public float maxY = 60f;

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        bool isDashing = playerMovement != null && playerMovement.IsDashing();

        if (!isDashing)
        {
            // Solo mover la cámara con el mouse si NO está en dash
            Vector2 mouseInput = Mouse.current.delta.ReadValue() * sensitivity * Time.deltaTime;
            yaw += mouseInput.x;
            pitch -= mouseInput.y;
            pitch = Mathf.Clamp(pitch, minY, maxY);
        }

        // Rotación actual (mantiene la última vista si está en dash)
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance + Vector3.up * offset.y;

        // Si NO está en dash, hacer raycast para evitar paredes
        if (!isDashing)
        {
            RaycastHit hit;
            Vector3 direction = (desiredPosition - target.position).normalized;

            if (Physics.Raycast(target.position + Vector3.up * 1.5f, direction, out hit, distance))
            {
                transform.position = hit.point - direction * 0.2f;
            }
            else
            {
                transform.position = desiredPosition;
            }
        }
        else
        {
            // Si está en dash, simplemente seguir sin raycast
            transform.position = desiredPosition;
        }

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }


}
