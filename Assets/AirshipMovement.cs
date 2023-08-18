using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipMovement : MonoBehaviour
{
    public float moveSpeed;
    public float rotSpeed;

    float hor, vert;

    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        hor = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");

        DoMove();
        //DoRot();

        if (vert == 0)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void DoMove()
    {
        //// Calculating Movement Direction, always walk in the direction you're looking
        //Vector3 moveDirection = transform.forward * vert;

        ////ac.SetFloat("moveSpeed", rb.velocity.magnitude);

        //// Adding Force to the Player Rigidbody if on Ground
        //rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        transform.Rotate(0, hor * 10 * Time.deltaTime, 0);
        transform.Translate(0, 0, vert * 10 * Time.deltaTime * moveSpeed);

        if (Input.GetKey(KeyCode.Space)&& transform.position.y < 50)
        {
            transform.Translate(0, 10 * Time.deltaTime * moveSpeed, 0);

        }
        else if (Input.GetKey(KeyCode.C) && transform.position.y > -50)
        {
            transform.Translate(0, -10 * Time.deltaTime * moveSpeed, 0);
        }

    }
}
