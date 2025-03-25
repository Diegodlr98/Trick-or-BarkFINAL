using UnityEngine;
using UnityEngine.InputSystem;

public class Animations : MonoBehaviour
{

    //private properties
    private Animator anim;
    public CharacterController characterController;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction dashAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        dashAction = InputSystem.actions.FindAction("Dash");
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAction.IsPressed())
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }

        if (jumpAction.WasPressedThisFrame())
        {
            anim.SetTrigger("jump");
        }

        if (sprintAction.IsPressed())
        {
            anim.SetBool("sprint", true);
        }
        else
        {
            anim.SetBool("sprint", false);
        }

        if (dashAction.WasPressedThisFrame())
        {
            anim.SetTrigger("Dash");
        }

            anim.SetBool("IsGrounded", characterController.isGrounded);
        
    }
    

}