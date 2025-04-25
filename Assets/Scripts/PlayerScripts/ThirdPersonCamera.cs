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

        // Movimiento de cámara solo si no está haciendo dash
        if (!isDashing)
        {
            Vector2 mouseInput = Mouse.current.delta.ReadValue() * sensitivity * Time.deltaTime;
            yaw += mouseInput.x;
            pitch -= mouseInput.y;
            pitch = Mathf.Clamp(pitch, minY, maxY);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance + Vector3.up * offset.y;
        Vector3 finalPosition = desiredPosition;

        // Aplica raycast para evitar colisiones, a menos que esté dashing
        if (!isDashing)
        {
            RaycastHit hit;
            Vector3 direction = (desiredPosition - target.position).normalized;

            if (Physics.Raycast(target.position + Vector3.up * 1.5f, direction, out hit, distance))
            {
                finalPosition = hit.point - direction * 0.2f;
            }
        }

        transform.position = finalPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }




}
