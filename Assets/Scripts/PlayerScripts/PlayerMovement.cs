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
    //private properties

    CharacterController controller;
    private bool canDash = true;
    
    
    //accions del mando/teclat

    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction rotAction;
    InputAction dashAction;

    float velocity; //valor coordenada Y en cada momento (altura)    
    Vector3 movement; //Coordenades del moviment total
        
    public GameObject video;
    public bool reproducir = false;
    public GameObject UI;

    public int speed = 10; //velocitat del personatge
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

        //con esto se cambia el comportamiento del salto en funcion de las chuches
        if (CandyCount < 15)
        {
            if (jumpAction.WasPressedThisFrame() && controller.isGrounded)
            {
                velocity = Mathf.Sqrt(jumpHeight * 2f * (9.8f * gravityScale)); //calcula el valor de la velocitat inicial para que al decrementarlo posteriormente llegue a la altura indicada                            
            }
        }
        if (CandyCount >= 15)
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
        velocity += -9.8f * gravityScale * Time.deltaTime; //decremento de la velocidad

        if (sprintAction.IsPressed() && controller.isGrounded)
        {
            sprint = 2;
        }
        else
        {
            sprint = 1;
        }
        //movimiento con rotacion en raton
        Vector2 lookInput = rotAction.ReadValue<Vector2>(); // obtiene la rotacion
        float horizontalRotation = lookInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, horizontalRotation, 0f); // Gira al player
        movement = velocity * Vector3.up + moveAction.ReadValue<Vector2>().x * transform.right * speed + moveAction.ReadValue<Vector2>().y * speed * sprint * transform.forward;

        controller.Move(movement * Time.deltaTime);

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
                    dashIcon.SetActive(true); // muestra el icono cuando hay suficientes chuches
                }
                else
                {
                    dashIcon.SetActive(false); // Oculta el icono hasta que se consiguen las suficientes chuches
                }
            }
        }
    
    private IEnumerator dashCoroutine()
    {
        if (!canDash) yield break;

        canDash = false; //el dash se bloquea mientras se ejecuta
        float startTime = Time.time;
        Vector3 dashDirection = transform.forward;

        if(dashIcon != null)
        {
            dashIcon.SetActive(false);
        }

        while (Time.time < startTime + dashTime)
        {
            controller.Move(dashDirection * Time.deltaTime * dashSpeed);
            yield return null;
            
           
        }
        yield return new WaitForSeconds(dashCooldown); //espera el cooldown antes de activarlo
        canDash = true; //el dash se habilita de nuevo
                       
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
            if(videoScript != null) 
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
        // Activa el VideoPlayer antes de esperar

        yield return new WaitForSeconds(2f);

        video.gameObject.SetActive(true);       
        UI.gameObject.SetActive(false);
        
        videoPlayer = video.GetComponent<VideoPlayer>();

        if (video != null)
        {           
            // Espera hasta que termine el video
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
