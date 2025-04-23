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

    float velocityY;
    Vector3 movement;

    public Transform cameraTransform;

    public GameObject video;
    public GameObject UI;
    public GameObject dashIcon;

    public int speed = 10;
    public int gravityScale = 10;
    public float jumpHeight = 2f;
    public float fallAcceleration = 2f;
    public float maxFallSpeed = -50f;
    private float fallVelocity = 0f;

    public float rotationSpeed = 5f;
    public float dashCooldown = 1f;
    public float dashTime = 1f;
    public float dashSpeed = 10f;

    public int maxJump = 1;
    public int CandyCount = 0;
    public int memoryCount = 0;

    public CandyUI candyUI;
    public MemoryUI memoryUI;

    private int jumpCount = 0;
    private int sprintMultiplier = 1;
    private VideoPlayer videoPlayer;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        dashAction = InputSystem.actions.FindAction("Dash");

        candyUI.UpdateCandyCount(CandyCount);
        memoryUI.UpdateMemoryCount(memoryCount);
        UpdateDashIconVisibility();
    }

    void Update()
    {
        HandleJump();
        HandleGravity();
        HandleSprint();
        HandleMovement();
        HandleDash();
    }

    void HandleJump()
    {
        if (jumpAction.WasPressedThisFrame())
        {
            if (CandyCount < 15 && controller.isGrounded)
            {
                velocityY = Mathf.Sqrt(jumpHeight * 2f * (9.8f * gravityScale));
            }
            else if (CandyCount >= 15 && jumpCount < maxJump)
            {
                velocityY = Mathf.Sqrt(jumpHeight * 2f * (9.8f * gravityScale));
                jumpCount++;
            }
        }

        if (controller.isGrounded)
        {
            jumpCount = 0;
        }
    }

    void HandleGravity()
    {
        if (controller.isGrounded && velocityY < 0)
        {
            fallVelocity = 0f;
            velocityY = -1f;
        }
        else
        {
            fallVelocity += fallAcceleration * Time.deltaTime;
            velocityY += -9.8f * gravityScale * fallVelocity * Time.deltaTime;
            velocityY = Mathf.Max(velocityY, maxFallSpeed);
        }
    }

    void HandleSprint()
    {
        sprintMultiplier = (sprintAction.IsPressed() && controller.isGrounded) ? 2 : 1;
    }

    void HandleMovement()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        if (input.sqrMagnitude > 0.01f)
        {
            // Dirección relativa a la cámara
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * input.y + camRight * input.x;
            moveDir.Normalize();

            // Movimiento del personaje
            Vector3 horizontalMove = moveDir * speed * sprintMultiplier;
            movement = horizontalMove + Vector3.up * velocityY;

            // Rotación suave hacia dirección de movimiento
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            movement = Vector3.up * velocityY;
        }

        controller.Move(movement * Time.deltaTime);
    }


    void HandleDash()
    {
        if (dashAction.WasPressedThisFrame() && CandyCount >= 25 && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    IEnumerator DashCoroutine()
    {
        canDash = false;
        Vector3 dashDirection = transform.forward;

        UpdateDashIconVisibility(false);

        float elapsedTime = 0f;
        while (elapsedTime < dashTime)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        UpdateDashIconVisibility(true);
    }

    void UpdateDashIconVisibility(bool show = true)
    {
        if (dashIcon != null)
        {
            dashIcon.SetActive(CandyCount >= 5 && canDash && show);
        }
    }

    void OnTriggerEnter(Collider other)
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
                StartCoroutine(videoScript.playMemory());
            }

            Destroy(other.gameObject);
            memoryCount++;
            memoryUI.UpdateMemoryCount(memoryCount);

            if (memoryCount >= 6)
                StartCoroutine(PlayMemoryVideo());
        }
    }

    IEnumerator PlayMemoryVideo()
    {
        yield return new WaitForSeconds(2f);

        video.SetActive(true);
        UI.SetActive(false);

        videoPlayer = video.GetComponent<VideoPlayer>();

        while (!videoPlayer.isPlaying)
            yield return null;

        while (videoPlayer.isPlaying)
            yield return null;

        SceneManager.LoadScene("Final");
    }
}
