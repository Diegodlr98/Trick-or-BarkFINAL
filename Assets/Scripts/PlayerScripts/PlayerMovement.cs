using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    private bool canDash = true;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction dashAction;

    float velocity;
    Vector3 movement;

    public GameObject video;
    public bool reproducir = false;
    public GameObject UI;

    public int speed = 10;
    public int gravityScale = 10;
    public float jumpHeight = 2f;
    public int jumpCount = 0;
    public int maxJump = 1;
    public int sprint = 0;
    public int CandyCount = 0;
    public float dashCooldown = 1f;
    public float dashTime = 1;
    public float dashSpeed = 10;
    public int memoryCount = 0;
    public float rotationSpeed = 5f;
    public CandyUI candyUI;
    public MemoryUI memoryUI;
    public GameObject dashIcon;

    private VideoPlayer videoPlayer;

    public float fallAcceleration = 2f;
    public float maxFallSpeed = -50f;
    private float fallVelocity = 0f;

    private Transform cam;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        dashAction = InputSystem.actions.FindAction("Dash");

        candyUI.UpdateCandyCount(CandyCount);
        memoryUI.UpdateMemoryCount(memoryCount);
        UpdateDashIconVisibility();

        cam = Camera.main.transform;
    }

    private void Update()
    {
        // Saltar
        if (CandyCount < 15)
        {
            if (jumpAction.WasPressedThisFrame() && controller.isGrounded)
            {
                velocity = Mathf.Sqrt(jumpHeight * 2f * (9.8f * gravityScale));
            }
        }
        else
        {
            if (jumpAction.WasPressedThisFrame() && jumpCount < 1)
            {
                velocity = Mathf.Sqrt(jumpHeight * 2f * (9.8f * gravityScale));
                jumpCount++;
            }
            if (controller.isGrounded)
            {
                jumpCount = 0;
            }
        }

        // Caída suavizada
        if (controller.isGrounded && velocity < 0)
        {
            fallVelocity = 0f;
            velocity = -1f;
        }
        else
        {
            if (velocity < 0 && !jumpAction.WasPressedThisFrame())
            {
                fallVelocity += fallAcceleration * 1.5f * Time.deltaTime;
            }
            else
            {
                fallVelocity += fallAcceleration * Time.deltaTime;
            }

            velocity += -9.8f * gravityScale * fallVelocity * Time.deltaTime;

            if (velocity < maxFallSpeed)
                velocity = maxFallSpeed;
        }

        // Sprint
        sprint = (sprintAction.IsPressed() && controller.isGrounded) ? 2 : 1;

        // Movimiento estilo tercera persona
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 camForward = cam.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cam.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 moveDir = (camForward * input.y + camRight * input.x).normalized;

        if (moveDir != Vector3.zero)
        {
            // Rotar suavemente hacia dirección de movimiento
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        float currentSpeed = (input.y < 0) ? speed * 0.7f : speed;
        movement = moveDir * currentSpeed * sprint + velocity * Vector3.up;

        controller.Move(movement * Time.deltaTime);

        // Dash
        if (dashAction.WasPressedThisFrame() && CandyCount >= 25)
        {
            StartCoroutine(dashCoroutine());
        }
    }

    private void UpdateDashIconVisibility()
    {
        if (dashIcon != null)
        {
            dashIcon.SetActive(CandyCount >= 5 && canDash);
        }
    }

    private IEnumerator dashCoroutine()
    {
        if (!canDash) yield break;

        canDash = false;
        float startTime = Time.time;
        Vector3 dashDirection = transform.forward;

        if (dashIcon != null)
        {
            dashIcon.SetActive(false);
        }

        while (Time.time < startTime + dashTime)
        {
            controller.Move(dashDirection * Time.deltaTime * dashSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

        if (dashIcon != null)
        {
            dashIcon.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Candy"))
        {
            Destroy(other.gameObject);
            CandyCount++;
            candyUI.UpdateCandyCount(CandyCount);
        }

        if (other.CompareTag("prueba"))
        {
            MemoryVideo videoScript = other.GetComponent<MemoryVideo>();
            if (videoScript != null)
            {
                StartCoroutine(other.GetComponent<MemoryVideo>().playMemory());
            }
            Destroy(other.gameObject);
            memoryCount++;
            memoryUI.UpdateMemoryCount(memoryCount);
            if (memoryCount >= 6)
            {
                StartCoroutine(PlayMemoryVideo());
            }
        }
    }

    private IEnumerator PlayMemoryVideo()
    {
        yield return new WaitForSeconds(2f);

        video.gameObject.SetActive(true);
        UI.gameObject.SetActive(false);

        videoPlayer = video.GetComponent<VideoPlayer>();

        if (video != null)
        {
            while (!videoPlayer.isPlaying)
            {
                yield return null;
            }

            while (videoPlayer.isPlaying)
            {
                yield return null;
            }

            SceneManager.LoadScene("Final");
        }
    }
}
