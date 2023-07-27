using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCMovementTest : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool grounded;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = controller.isGrounded;

        if(grounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, (Input.GetAxis("Vertical")));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if(Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Jump();
        }

        playerVelocity.y = gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void Jump()
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }
}
