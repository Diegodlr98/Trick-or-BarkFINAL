
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.XR.GoogleVr;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerMovement : MonoBehaviour
{
    // Private properties
    CharacterController controller;
    private bool canDash = true;

    // Acciones del mando/teclado
    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction rotAction;
    InputAction dashAction;

    float velocity; // valor coordenada Y en cada momento (altura)
    Vector3 movement; // Coordenadas del movimiento total

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
    public float rotationSpeed = 100;
    public int memoryCount = 0;
    public CandyUI candyUI;
    public MemoryUI memoryUI;
    public GameObject dashIcon;

    private VideoPlayer videoPlayer;

    // NUEVAS VARIABLES para caída suave
    public float fallAcceleration = 2f;
    public float maxFallSpeed = -50f;
    private float fallVelocity = 0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        rotAction = InputSystem.actions.FindAction("Look");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        dashAction = InputSystem.actions.FindAction("Dash");

        candyUI.UpdateCandyCount(CandyCount);
        memoryUI.UpdateMemoryCount(memoryCount);
        UpdateDashIconVisibility();
    }

    private void Update()
    {
        // Salto
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

        // CAÍDA SUAVIZADA
        if (controller.isGrounded && velocity < 0)
        {
            fallVelocity = 0f;
            velocity = -1f; // pequeña presión hacia abajo
        }
        else
        {
            // Aceleración progresiva de caída
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
        if (sprintAction.IsPressed() && controller.isGrounded)
        {
            sprint = 2;
        }
        else
        {
            sprint = 1;
        }

        // Rotación con ratón
        Vector2 lookInput = rotAction.ReadValue<Vector2>();
        float horizontalRotation = lookInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, horizontalRotation, 0f);

        // Movimiento general
        movement = velocity * Vector3.up +
                   moveAction.ReadValue<Vector2>().x * transform.right * speed +
                   moveAction.ReadValue<Vector2>().y * speed * sprint * transform.forward;

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
            if (CandyCount >= 5 && canDash)
            {
                dashIcon.SetActive(true);
            }
            else
            {
                dashIcon.SetActive(false);
            }
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

