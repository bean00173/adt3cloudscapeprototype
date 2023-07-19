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
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    public Transform orientation;

    public bool dashing;

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
        dashing,
        idle
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
