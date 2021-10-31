using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 10;
    [SerializeField] private float sprintSpeed = 17;
    [SerializeField] private int facingDirection = 1;
    [SerializeField] private bool isThicc = false;
    private Rigidbody2D rb; 
    private SpriteRenderer spriteRenderer; 
    private Vector2 movementVel;
    private Vector2 inputDir;
    private Animator animator;
    
    RaphlInputActions raphlInputActions;

    public bool isSprinting;


    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.raphlInputActions = new RaphlInputActions();

        raphlInputActions.Player.Enable();
        // raphlInputActions.Player.Move.performed += CheckWalk;

        raphlInputActions.Player.SprintStart.performed += x => SprintPressed();
        raphlInputActions.Player.SprintFinish.performed += x => SprintReleased();
        raphlInputActions.Player.Quit.performed += x => Quit();
        raphlInputActions.Player.Upsize.performed += x => Upscale();
    }

    // Update is called once per frame
    void Update()
    {
        CheckWalk();
        CheckAnimatorParams();
    }
    
    private void FixedUpdate()
    {
        Move();
    }

    // public void CheckWalk(InputAction.CallbackContext context)
    // {
    //     // Debug.Log("Walk" + context);
    //     if (context.performed)
    //     {
    //         this.inputDir = context.ReadValue<Vector2>();
    //         this.movementX = new Vector2(inputDir.x * speed, 0);
    //     }
    // }

    private void CheckWalk()
    {
        this.inputDir = raphlInputActions.Player.Move.ReadValue<Vector2>();
        if (inputDir.x != 0 && inputDir.x != facingDirection)
        {
            Flip();
        }
        if (isSprinting)
        {
            this.movementVel = new Vector2(inputDir.x * sprintSpeed, rb.velocity.y);
        }
        else {
            this.movementVel = new Vector2(inputDir.x * walkSpeed, rb.velocity.y);
        }
    }

    // private void CheckQuit()
    // {
    //     if (raphlInputActions.Player.Quit.)
    //     {

    //     }
    // }

    private void Move()
    {
        rb.velocity = movementVel;
    }

    private void SprintPressed()
    {
        isSprinting = true;
    }
    private void SprintReleased()
    {
        isSprinting = false;
    }

    private void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    private void Upscale()
    {
        if (!isThicc)
        {
            transform.localScale = new Vector3(2.5f,2.5f,1);
            isThicc = true;
        }
        else {
            transform.localScale = new Vector3(1,1,1);
            isThicc = false;
        }
    }

    private void CheckAnimatorParams()
    {
        if (IsMoving())
        {
            animator.SetBool("isWalking", true);
        }
        else {
            animator.SetBool("isWalking", false);
        }

        if (isSprinting)
        {
            animator.SetBool("isSprinting", true);
        }
        else {
            animator.SetBool("isSprinting", false);
        }
    }

    private bool IsMoving()
    {
        if (this.movementVel.x != 0)
        {
            return true;
        }
        else {
            return false;
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        rb.transform.Rotate(0.0f, 180f, 0.0f);
    }

    public bool IsFacingRight()
    {
        if (facingDirection == 1) { return true; }
        else {  return false; }
    }

    public float GetInputDirX()
    {
        return inputDir.x;
    }
    public bool IsSprinting()
    {
        return isSprinting;
    }
}
