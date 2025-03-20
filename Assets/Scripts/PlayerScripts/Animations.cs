using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehav : MonoBehaviour
{

    //private properties
    private Animator anim;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction dashAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
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

        if (jumpAction.IsPressed())
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
        if(dashAction.IsPressed())
        {
            anim.SetTrigger("Dash");
        }
    }
}