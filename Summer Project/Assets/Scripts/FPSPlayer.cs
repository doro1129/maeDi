using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is a script that detects and controls the movement of the player.
/// </summary>
public class FPSPlayer : MonoBehaviour
{
    /// <summary>
    /// orientation will store the direction where the player facing
    /// </summary>
    public Transform orientation;

    private Rigidbody rigidbody1;

    // Player's numerical values of attack and stamina
    [Header("Player")]

    /// <summary>
    /// Player's striking power, cooldown time and speed of attack when attacking
    /// </summary>
    public float Striking_Power = 50;   
    public float Attack_Time = 1f;      
    public float Attack_Speed = 1f;
    
    /// <summary>
    /// Player's stamina as known as 'HP'
    /// </summary>
    public float Stamina = 100;

    // Player's moddable numerical values of movement like waliking speed, jump force
    [Header("Movement")]

    /// <summary>
    /// Player's speed when player walk and run
    /// </summary>
    public float MoveSpeed = 25;
    public float RunSpeed = 50;

    /// <summary>
    /// Resistance of ground when moving
    /// The higher the value, the stronger the resistance of the ground on which the player walks.
    /// </summary>
    public float GroundDrag = 7;

    /// <summary>
    /// Player's jumping power
    /// </summary>
    public float JumpForce = 30;

    /// <summary>
    /// Cooldown time of jumping
    /// It will invoke the function(ResetJump) after this value of time.
    /// </summary>
    public float JumpCooldown = 0.5f;

    /// <summary>
    /// AirMultiplier is the value that decreases the speed when player is floating
    /// </summary>
    public float AirMultiplier =0.4f;

    private bool ReadyToJump = true;
    

    [Header("Keybinds")]
    private KeyCode JumpKey = KeyCode.Space;

    //To check if player is on the ground
    [Header("Ground Check")]
    /// <summary>
    /// To check the layer which is named 'what is ground'
    /// </summary>
    public LayerMask Tile;

    /// <summary>
    /// It will 'True' when player is on ground or false when floating
    /// </summary>
    public bool grounded;
    public float RaycastDistance = 10;

    private Vector3 moveDirection;
    private float horizontalInput;
    private float verticalInput;
    


    private void Start()
    {
        rigidbody1 = GetComponent<Rigidbody>();
        rigidbody1.freezeRotation = true;   // not to fall down
    }

    private void Update()
    {   
        // Check if it is attaching at the ground by using raycast as type of boolean
        grounded = Physics.Raycast(transform.position, Vector3.down, RaycastDistance, Tile);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
        {
            rigidbody1.drag = GroundDrag;
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
        else
        {
            rigidbody1.drag = 1;
            Physics.gravity = new Vector3(0, -60f, 0);
        }
    }
    
    private void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// Detect the value of Horizontal and Vertical input
    /// If press the 'jump key(space)' player will jump
    /// </summary>
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(JumpKey) && ReadyToJump && grounded)
        {
            ReadyToJump = false;
            Jump();

            // Reset readyToJump to true
            Invoke(nameof(ResetJump), JumpCooldown);
        }
    }

    /// <summary>
    /// Calculate the movement direction
    /// When on the ground or in the air, it gives the proper force for the situation.
    /// </summary>
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            var currentSpeed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : MoveSpeed;

            rigidbody1.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
        }

        //in air
        else
        {
            rigidbody1.AddForce(moveDirection.normalized * MoveSpeed * 10f * AirMultiplier, ForceMode.Force);
        }
    }

    /// <summary>
    /// Manually limit the speed of the player
    /// </summary>
    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rigidbody1.velocity.x, 0f, rigidbody1.velocity.z);

        //limit velocity if needed
        if (flatVelocity.magnitude > MoveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * MoveSpeed;
            rigidbody1.velocity = new Vector3(limitedVelocity.x, rigidbody1.velocity.y, limitedVelocity.z);
        }
    }

    /// <summary>
    /// Jump function when 'Jump Key' is pressed
    /// </summary>
    private void Jump()
    {
        rigidbody1.velocity = new Vector3(rigidbody1.velocity.x, 0, rigidbody1.velocity.z);
        rigidbody1.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Reset 'ReadyToJump' to true
    /// </summary>
    private void ResetJump()
    {
        ReadyToJump = true;
    }
}
