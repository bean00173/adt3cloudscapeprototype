using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]

    private float movementSpeed;
    public float walkMultiplier;
    public float sprintMultiplier;
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

    public bool canMove = true;

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
            movementSpeed = sprintMultiplier * this.GetComponent<PlayableCharacter>().speedModifier;
        } // Mode - Walking
        else if (grounded && (horizontalInput != 0 || verticalInput != 0))
        {
            state = MovementState.walking;
            movementSpeed = walkMultiplier * this.GetComponent<PlayableCharacter>().speedModifier;
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

        foreach(Transform child in this.transform.GetChild(0))
        {
            child.GetComponent<OnStep>().onStep.AddListener(DoStep);
        }
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

        switch (state)
        {
            case MovementState.walking: this.GetComponent<PlayableCharacter>().ac.SetFloat("speed", 1f); break;
            case MovementState.sprinting: this.GetComponent<PlayableCharacter>().ac.SetFloat("speed", 2f); break;
            default: this.GetComponent<PlayableCharacter>().ac.SetFloat("speed", 0f); break;
        }

        // Calling Input Function & Speed Control on Every Frame Update

        if (canMove)
        {
            MyInput();
            SpeedControl();
        }
        else
        {
            rb.velocity = new Vector3(0, -9.81f, 0);
            rb.angularVelocity = Vector3.zero;

            //transform.position = this.GetComponent<PlayableCharacter>().ac.deltaPosition;
            //transform.rotation = this.GetComponent<PlayableCharacter>().ac.deltaRotation;
        }

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
            this.GetComponent<PlayableCharacter>().ac.SetTrigger("Jump");
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

    public void MoveInterrupt(bool move)
    {
        if (!move)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }

    private void DoStep()
    {
        string clip = "";
        switch (this.GetComponent<PlayableCharacter>().currentCharacter)
        {
            case Character.CharacterId.seb: clip = "Seb_Step"; break;
            case Character.CharacterId.abi: clip = "Abi_Step"; break;
            case Character.CharacterId.rav: clip = "Rav_Step"; break;
        }
        this.GetComponent<SoundHandler>().PlaySound(clip);
    }
}
