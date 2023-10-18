using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AirshipMovement : MonoBehaviour
{
    public float boostSpeed;
    public float defaultSpeed;
    private float moveSpeed;
    public float rotSpeed;

    bool moving, ascending, descending, turnLeft, turnRight;

    public AudioSource upDown;

    float hor, vert;

    Rigidbody rb;
    public Animator ac;

    [HideInInspector]
    public bool canMove;

    [HideInInspector] public bool emitting;
    public GameObject cloudsPS;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        ac = GetComponentInChildren<Animator>();

        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        //moveSpeed = Input.GetKey(KeyCode.LeftShift) ? boostSpeed : defaultSpeed;
        moveSpeed = defaultSpeed;

        if (canMove)
        {
            hor = Input.GetAxisRaw("Horizontal");
            vert = Input.GetAxisRaw("Vertical");

            moving = vert != 0 && !descending && !ascending ? true : false;
            ac.SetBool("moving", moving);
            ac.SetBool("turnLeft", turnLeft);
            ac.SetBool("turnRight", turnRight);
            ac.SetBool("ascend", ascending);
            ac.SetBool("descend", descending);

            //DoMove();
            ////DoRot();

            //if (vert == 0)
            //{
            //    rb.velocity = Vector3.zero;
            //    rb.angularVelocity = Vector3.zero;
            //}

            switch (hor)
            {
                case < 0: turnLeft = true; turnRight = false; break;
                case > 0: turnRight = true; turnLeft = false; break;
                default: turnRight = false; turnLeft = false; break;
            }
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            //hor = Input.GetAxisRaw("Horizontal");
            //vert = Input.GetAxisRaw("Vertical");

            //ac.SetFloat("horizontalInput", hor);

            DoMove();
            //DoRot();

            if (vert == 0)
            {
                if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift))
                {
                    
                }
                else
                {
                    
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }
    }

    private void DoMove()
    {
        //// Calculating Movement Direction, always walk in the direction you're looking
        //Vector3 moveDirection = transform.forward * vert;

        ////ac.SetFloat("moveSpeed", rb.velocity.magnitude);

        //// Adding Force to the Player Rigidbody if on Ground
        //rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        if(vert > 0)
        {
            ac.SetBool("moving", true);
            if (!emitting)
            {
                emitting = true;
                cloudsPS.GetComponent<ParticleSystem>().Play();
            }
            
        }
        else
        {
            ac.SetBool("moving", false);
            emitting = false;
            cloudsPS.GetComponent<ParticleSystem>().Stop();
        }

        //transform.Rotate(0, hor * 10 * Time.deltaTime, 0);
        //transform.Translate(0, 0, vert * 10 * Time.deltaTime * moveSpeed);

        Vector3 dir = transform.TransformDirection(new Vector3(0, 0, vert * 10 * Time.fixedDeltaTime));
        rb.velocity = dir * 50 * moveSpeed;

        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, hor * rotSpeed * Time.fixedDeltaTime, 0));
        rb.MoveRotation(rb.rotation * deltaRotation);

        ac.SetFloat("elevation", 0);

        if (Input.GetKey(KeyCode.Space) && transform.position.y < 50)
        {
            ascending = true;
            if (!upDown.isPlaying) upDown.Play();
            Vector3 vertDir = new Vector3(0, moveSpeed * 10 * Time.fixedDeltaTime, 0);
            Debug.Log(vertDir);
            rb.velocity += vertDir * 50f;
            ac.SetFloat("elevation", 1);

        }
        else if (Input.GetKey(KeyCode.LeftShift) && transform.position.y > -50)
        {
            descending = true;
            if (!upDown.isPlaying) upDown.Play();
            Vector3 vertDir = transform.TransformDirection(new Vector3(0, moveSpeed * -10 * Time.fixedDeltaTime, 0));
            rb.velocity += vertDir * 50f;
            ac.SetFloat("elevation", -1);
        }
        else
        {
            ascending = false;
            descending = false;
            upDown.Stop();
        }

    }

    public void IslandRepel(Transform island, float force)
    {
        Vector3 aimTarget = island.position;
        Vector3 target = new Vector3(aimTarget.x - this.transform.position.x, 0f, aimTarget.z - this.transform.position.z);
        target.Normalize();
        Vector3 velocity = target * -force;
        rb.AddForce(velocity);
    }

    public void DisableMovement(bool disabled)
    {
        if(disabled)
        {
            canMove = false;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            canMove = true;
        }
    }
}
