using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingScript : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pc;

    [Header("Dashing")]
    public float rollForce;
    public float rollDuration;

    [Header("Cooldown")]
    public float rollCd;
    private float rollCdTimer;

    private Animator ac;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        pc = this.GetComponent<PlayerMovement>();
        ac = this.GetComponentInChildren<Animator>();
        
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Dash();
        }

        if(rollCdTimer > 0)
        {
            rollCdTimer -= Time.deltaTime;
        }
    }

    private void Dash()
    {

        if (rollCdTimer > 0) { return; }
        else { rollCdTimer = rollCd; }

        pc.rolling = true;

        pc.groundDrag = 0;

        Vector3 forceToApply = orientation.forward * rollForce;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), rollDuration);
    }

    private Vector3 delayedForceToApply;

    private void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pc.rolling = false;
        pc.notDashing();
        pc.groundDrag = 5;
    }

    public void ResetCD()
    {
        rollCdTimer = 0;
    }

}

