using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]

    private float movementSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    public float dashSpeed;


    [Header("Jumping")]
    public float jumpForce;
    public float airMultiplier;
    public float jumpCooldown;
    bool jumpReady;

    [Header("Ground Check")]
    public LayerMask whatIsGround;
    public bool grounded;

    public Transform orientation;

    public bool rolling;

    float horizontalInput;
    float verticalInput;

    [Header("Audio")]
    [Tooltip("Put AudioClips in the order, Dashing, WallRunning, Sprinting, Walking, JumpStart, JumpLand")]
    [SerializeField] private AudioClip[] _clips;
    AudioSource source;
    int _clipIndex;
    int lastIndex = 0;

    Vector3 moveDirection;

    Rigidbody rb;
    Animator ac;

    bool[] states;

    public MovementState state;

    public enum MovementState
    {
        sprinting,
        walking,
        air,
        rolling,
        idle
    }

    private void StateHandler()
    {
        // Mode - rolling
        if (rolling)
        {
            state = MovementState.rolling;
            movementSpeed = dashSpeed;
            //ac.SetBool("dashing", true);
        }
        // Mode - Sprinting
        else if (grounded && Input.GetKey(KeyCode.LeftShift) && (horizontalInput != 0 || verticalInput != 0))
        {
            state = MovementState.sprinting;
            movementSpeed = sprintSpeed;
        } // Mode - Walking
        else if (grounded && (horizontalInput != 0 || verticalInput != 0))
        {
            state = MovementState.walking;
            movementSpeed = walkSpeed;
        }
        else if (grounded) // Mode - Idle
        {
            state = MovementState.idle;
            movementSpeed = 0;
            source.Stop();
        }
        else if (!grounded) // Mode - Air
        {
            state = MovementState.air;
        }
    }

    private void Start()
    {
        // Collecting Rigidbody and freezing the Rotation
        rb = this.GetComponent<Rigidbody>();
        //ac = this.transform.Find("Player").GetComponent<Animator>();
        source = this.GetComponent<AudioSource>();
        rb.freezeRotation = true;
        ResetJump();
    }
    public void SetAudioClip(int index)
    {
        _clipIndex = index;
    }

    // Update is called once per frame
    void Update()
    {
        // Checking if the player is grounded

        grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f, whatIsGround);
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down, Color.red, 0.2f);

        // Calling Input Function & Speed Control on Every Frame Update

        MyInput();
        SpeedControl();
        StateHandler();

        //ac.SetFloat("verticalSpeed", rb.velocity.y);
    }

    private void FixedUpdate()
    {
        // Calling Move Player
        MovePlayer();
    }

    private void MyInput()
    {
        // Detecting Key Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Check if conditions for Jump Met
        if (Input.GetKey(KeyCode.Space) && grounded && jumpReady)
        {
            jumpReady = false;
            Jump();

            // Invokes Method after given Time;
            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }

    private void MovePlayer()
    {
        // Calculating Movement Direction, always walk in the direction you're looking
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //ac.SetFloat("moveSpeed", rb.velocity.magnitude);

        // Adding Force to the Player Rigidbody if on Ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
        }

    }

    private void SpeedControl()
    {
        // Calculate what current Velocity is 
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit Velocity if Needed - Check if Current Velocity is greater than max velocity
        if (flatVel.magnitude > movementSpeed)
        {
            // Limit Velocity;
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity, to ensure jump height is always same
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        jumpReady = true;
    }

    public void notDashing()
    {
        //ac.SetBool("dashing", false);
    }

    public void PlayAudio(int index)
    {
        if (!source.isPlaying || index != lastIndex)
        {
            source.clip = _clips[index];
            source.Play();
        }

        lastIndex = index;
    }
}
