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
        // Movimiento de cámara con el mouse
        Vector2 mouseInput = Mouse.current.delta.ReadValue() * sensitivity * Time.deltaTime;
        yaw += mouseInput.x;
        pitch -= mouseInput.y;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        // Rotación y posición fija
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance + Vector3.up * offset.y;

        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
