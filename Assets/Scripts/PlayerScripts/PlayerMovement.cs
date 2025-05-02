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
    InputAction nextAction;
    float velocityY;
    Vector3 movement;

    public Transform cameraTransform;
    public GameObject video;
    public GameObject UI;
    public GameObject dashIcon;
    public GameObject skipMessage;

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
    public int jumpCount = 0;
    private int sprintMultiplier = 1;
    private VideoPlayer videoPlayer;
    private bool isDashing = false;
    private bool isRunning = false;
    private Quaternion dashRotation;

    public Camera playerCamera;
    public Camera eventCamera;
    public float eventCameraDuration = 3f;

    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter = 0f;

    [Header("SFX")]
    public AudioClip jumpSFX;
    public AudioClip dashSFX;
    public AudioSource sfxAudioSource;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        dashAction = InputSystem.actions.FindAction("Dash");
        nextAction = InputSystem.actions.FindAction("Next");
        nextAction.Enable();

        if (skipMessage != null)
            skipMessage.SetActive(false);
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
        if (controller.isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            jumpCount = 0;
            fallVelocity = 0f;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpAction.WasPressedThisFrame())
        {
            if (coyoteTimeCounter > 0f)
            {
                velocityY = Mathf.Sqrt(jumpHeight * 2f * (9.8f * gravityScale));
                jumpCount = 1;
                coyoteTimeCounter = 0f;
                PlaySFX(jumpSFX); // sonido de salto
            }
            else if (CandyCount >= 15 && jumpCount < maxJump)
            {
                velocityY = Mathf.Sqrt(jumpHeight * 2f * (9.8f * gravityScale));
                fallVelocity = 0f;
                jumpCount++;
                PlaySFX(jumpSFX); // sonido de doble salto
            }
        }
    }

    void HandleGravity()
    {
        if (isDashing) return;

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
        isRunning = true;
        sprintMultiplier = (sprintAction.IsPressed() && controller.isGrounded) ? 2 : 1;
        isRunning = false;
    }

    void HandleMovement()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        if (input.sqrMagnitude > 0.01f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * input.y + camRight * input.x;
            moveDir.Normalize();

            Vector3 horizontalMove = moveDir * speed * sprintMultiplier;
            movement = horizontalMove + (isDashing ? Vector3.zero : Vector3.up * velocityY);

            if (!isDashing)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = dashRotation;
            }
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
        isDashing = true;
        dashRotation = transform.rotation;

        Vector3 dashDirection = transform.forward;
        UpdateDashIconVisibility(false);

        PlaySFX(dashSFX); // sonido de dash

        float elapsedTime = 0f;
        while (elapsedTime < dashTime)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
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
            CandyCount++;
            candyUI.UpdateCandyCount(CandyCount);
            StartCoroutine(DestroyAfterSound(other.gameObject));
        }

        if (other.CompareTag("prueba"))
        {
            MemoryVideo videoScript = other.GetComponent<MemoryVideo>();
            if (videoScript != null)
            {
                memoryCount++;
                memoryUI.UpdateMemoryCount(memoryCount);

                videoScript.playerMovement = this;
                StartCoroutine(videoScript.playMemory());
            }

            StartCoroutine(DestroyAfterSound(other.gameObject));
        }
    }

    IEnumerator DestroyAfterSound(GameObject obj)
    {
        AudioSource source = obj.GetComponent<AudioSource>();
        if (source != null && source.clip != null)
        {
            GameObject tempAudio = new GameObject("TempAudio");
            AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
            tempSource.outputAudioMixerGroup = source.outputAudioMixerGroup;
            tempSource.clip = source.clip;
            tempSource.Play();
            obj.SetActive(false);
            Destroy(obj);
            Destroy(tempAudio, tempSource.clip.length);
        }
        else
        {
            Destroy(obj);
        }

        yield return null;
    }

    public IEnumerator PlayMemoryVideo()
    {
        yield return new WaitForSeconds(2f);

        video.SetActive(true);
        UI.SetActive(false);

        if (skipMessage != null)
            skipMessage.SetActive(true);

        videoPlayer = video.GetComponent<VideoPlayer>();
        videoPlayer.Play();

        while (!videoPlayer.isPlaying)
            yield return null;

        while (videoPlayer.isPlaying)
        {
            if (nextAction.WasPressedThisFrame())
            {
                videoPlayer.Stop();
                break;
            }
            yield return null;
        }

        if (skipMessage != null)
            skipMessage.SetActive(false);

        video.SetActive(false);
        UI.SetActive(true);

        SceneManager.LoadScene("Final");
    }

    public bool IsDashing() => isDashing;
    public bool IsRunning() => isRunning;

    void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(clip);
        }
    }
}
