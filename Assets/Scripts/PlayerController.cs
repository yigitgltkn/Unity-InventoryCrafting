using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Header("Movement Sound")]
    private AudioSource audioS;
    public AudioClip[] stepSounds;
    public float footStepRate, footStepThreshHold;
    private float lastStepTime;

    [Header("Jumping")]
    public float jumpForce;
    public LayerMask groundLayer;

    [Header("Movement")]
    public float moveSpeed = 6f;  //how fast our player should move
    private Vector2 currentMovementInput;
    [Header("Camera Look")]
    public Transform cameraContainer; //access to transform value of our camera container
    public float minXLook, maxXLook; //min&max amount that we can look down or up to avoid spin around completely
    private float camCurrentXRotation; // get access to camera rotation
    public float lookSensitivity;   // how fast we are able to look around
    private Vector2 mouseDelta;
    private Rigidbody myRig;
    [HideInInspector]
    public bool canLook = true;
    private void Awake()
    {
        instance = this;
        myRig = GetComponent<Rigidbody>();
        audioS = GetComponent<AudioSource>();
    }
    private void Start()
    {
        //locked the cursor and make it also invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    //LateUpdate is called after all Update functions have been called
    private void LateUpdate()
    {
        if (canLook == true)
        {
            // we want to rotate camera after player has moved
            CameraLook();
        }

    }

    //FixedUpdate will be called 50 time per second (around 0.02 second)
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        //store our movement value to move direction
        Vector3 moveDirection = transform.forward * currentMovementInput.y + transform.right * currentMovementInput.x;
        //move player with the speed valu of our moveSpeed
        moveDirection *= moveSpeed;
        moveDirection.y = myRig.velocity.y;
        myRig.velocity = moveDirection;

        //The velocity vector of the rigidbody. It represents the rate of change of Rigidbody position.
        //Time.time simply gives you a numeric value which is equal to
        //the number of seconds which have elapsed since the project started playing.

        //check if rigidbody speed bigger than our footstepthreshhold
        if (moveDirection.magnitude > footStepThreshHold && IsGrounded())
        {
            //real time minus last step time bigger than foot step rate
            if (Time.time - lastStepTime > footStepRate)
            {
                lastStepTime = Time.time;
                //play sound step
                audioS.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
            }
        }
    }

    //context contains all information we need to know such as we pressed the button or not ,the value or ....
    public void OnLookInput(InputAction.CallbackContext context)
    {
        // mouse delta is whatever is the value of vector2
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        //check if we pressing the button and if its true
        if (context.phase == InputActionPhase.Performed)
        {
            //move the player 
            currentMovementInput = context.ReadValue<Vector2>();
        }
        //check if we stop pressing the button and if its true
        else if (context.phase == InputActionPhase.Canceled)
        {
            //stop moving the player
            currentMovementInput = Vector2.zero;
        }
    }



    private void CameraLook()
    {
        //add value to camera x rotaion depend on our mouse delta y value then multiply it by sensitivity value
        camCurrentXRotation += mouseDelta.y * lookSensitivity;
        //limiting to looking up and down
        camCurrentXRotation = Mathf.Clamp(camCurrentXRotation, minXLook, maxXLook);
        //apply cam current x rotation to our camera container
        cameraContainer.localEulerAngles = new Vector3(-camCurrentXRotation, 0, 0);
        //rotate only along Y axis for the player Container
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        //is it the first frame that we held the button down 
        if (context.phase == InputActionPhase.Started)
        {
            //did we standing on the ground
            if (IsGrounded())
            {
                //add force toward upside
                myRig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }



    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward* 0.2f)+(Vector3.up * 0.02f),Vector3.down),
            new Ray(transform.position + (-transform.forward* 0.2f)+(Vector3.up * 0.02f),Vector3.down),
            new Ray(transform.position + (transform.right* 0.2f)+(Vector3.up * 0.02f),Vector3.down),
            new Ray(transform.position + (-transform.right* 0.2f)+(Vector3.up * 0.02f),Vector3.down)
        };

        //Statement 1 sets a variable before the loop starts (int i = 0).
        //Statement 2 defines the condition for the loop to run.If the condition is true, the loop will start over again, if it is false, the loop will end.
        //Statement 3 increases a value(i++) each time the code block in the loop has been executed.

        //loop through rays 
        for (int i = 0; i < rays.Length; i++)
        {
            //if rays hit the ground layer 
            if (Physics.Raycast(rays[i], 0.1f, groundLayer))
            {
                //player is on the ground
                return true;
            }
        }

        //player is not on the ground
        return false;
    }

    private void OnDrawGizmos()
    {
        //make the color of gizmos to red
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (transform.right * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.right * 0.2f), Vector3.down);
    }

    public void ToggleCursor(bool toggle)
    {
        //check if toggle is true then lockstate is none else lockstate is locked
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        //canLook is become opposite of toggle
        canLook = !toggle;
    }
}
