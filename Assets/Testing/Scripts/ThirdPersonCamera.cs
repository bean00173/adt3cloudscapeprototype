using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]

    public Transform orientation;
    public Transform player;

    public Transform camHolder;

    private CinemachineFreeLook cfl;
    PlayerMovement pm;

    public float rotationSpeed;
    int i = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cfl = this.GetComponent<CinemachineFreeLook>();
        pm = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // rotate player object
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // If Input Direction is not Zero and movement is enabled, change view direction to input direction
        if (inputDir != Vector3.zero && pm.canMove)
        {
            player.forward = Vector3.Slerp(player.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

    }


    public void DoFov() // currently unused spare function that uses a dotween transition
    {
        if(i == 0)
        {
            camHolder.GetChild(1).gameObject.SetActive(true);
            camHolder.GetChild(0).gameObject.SetActive(false);
            i++;
        }
        else if(i == 1)
        {
            camHolder.GetChild(1).gameObject.SetActive(false);
            camHolder.GetChild(0).gameObject.SetActive(true);
            i = 0;
        }

    }

}
