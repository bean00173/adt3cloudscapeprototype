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
        DoRot();

        if (vert == 0)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void DoMove()
    {
        // Calculating Movement Direction, always walk in the direction you're looking
        Vector3 moveDirection = transform.forward * vert + transform.right * hor;

        //ac.SetFloat("moveSpeed", rb.velocity.magnitude);

        // Adding Force to the Player Rigidbody if on Ground
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

    }

    private void DoRot()
    {
        Vector3 current = transform.position;
        Vector3 input = new Vector3(hor, 0, 0);

        input += current;

        Quaternion target = Quaternion.LookRotation(input - current);

        Debug.Log(target);

        transform.rotation = Quaternion.Slerp(transform.rotation, target, rotSpeed);
    }
}
