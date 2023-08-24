using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipMovement : MonoBehaviour
{
    public float moveSpeed;
    public float rotSpeed;

    float hor, vert;

    Rigidbody rb;
    Animator ac;

    bool canMove;


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
        if (canMove)
        {
            hor = Input.GetAxisRaw("Horizontal");
            vert = Input.GetAxisRaw("Vertical");

            ac.SetFloat("horizontalInput", hor);

            DoMove();
            //DoRot();

            if (vert == 0)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
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
        if(vert != 0)
        {
            ac.SetBool("moving", true);
        }
        else
        {
            ac.SetBool("moving", false);
        }

        transform.Rotate(0, hor * 10 * Time.deltaTime, 0);
        transform.Translate(0, 0, vert * 10 * Time.deltaTime * moveSpeed);

        //Quaternion deltaRot = Quaternion.Euler(new Vector3(0, hor * 10 * Time.fixedDeltaTime, 0));
        //rb.MoveRotation(rb.rotation * deltaRot);

        //Vector3 moveDir = new Vector3(0, 0, vert * 10 * Time.fixedDeltaTime * moveSpeed);
        //moveDir = rb.rotation * moveDir;
        //rb.MovePosition(this.transform.position + moveDir);

        ac.SetFloat("elevation", 0);

        if (Input.GetKey(KeyCode.Space)&& transform.position.y < 50)
        {
            //rb.MovePosition(new Vector3(0, 10 * Time.fixedDeltaTime * moveSpeed, 0));
            transform.Translate(0, 10 * Time.deltaTime * moveSpeed, 0);
            ac.SetFloat("elevation", 1);

        }
        else if (Input.GetKey(KeyCode.C) && transform.position.y > -50)
        {
            //rb.MovePosition(new Vector3(0, -10 * Time.fixedDeltaTime * moveSpeed, 0));
            transform.Translate(0, -10 * Time.deltaTime * moveSpeed, 0);
            ac.SetFloat("elevation", -1);
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