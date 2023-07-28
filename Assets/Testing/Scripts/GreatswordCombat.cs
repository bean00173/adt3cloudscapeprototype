using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatswordCombat : MonoBehaviour
{
    public Transform weapon;
    public Transform hitbox;
    Collider[] hitBoxes;
    Collider hit;
    PlayableCharacter pc;
    Animator ac;
    PlayerMovement pm;
    Rigidbody rb;

    bool readyToAtk = true;
    bool readyToHold = true;
    bool holding;

    public float holdCd;

    bool crRunning;

    float lastPressedTime;
    int comboIndex;
    public float comboTime = .25f;
    bool firstAtk;
    bool secondAtk;

    // Start is called before the first frame update
    void Start()
    {
        hitBoxes = hitbox.GetComponentsInChildren<Collider>();
        ac = this.transform.root.GetComponent<Animator>();
        pm = this.GetComponent<PlayerMovement>();
        rb = this.GetComponent<Rigidbody>();
        pc = this.GetComponent<PlayableCharacter>();

        foreach(Collider collider in hitBoxes)
        {
            collider.enabled = false;
            collider.isTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        hit = hitBoxes[comboIndex];
        //if (Input.GetKey(KeyCode.Mouse0))
        //{
        //    Debug.Log("HOLDING ATTACK");
        //}
        if (Input.GetKey(KeyCode.F) && readyToHold)
        {
            //StartCoroutine(SpinAttackMov());
            pm.MoveInterrupt(false);
            holding = true;
            readyToAtk = false;
            hit = hitBoxes[0];
            hit.enabled = true;
            ac.SetBool("holdAtk", true);
        }
        else if(holding == true)
        {
            this.transform.rotation = pm.orientation.transform.rotation;
            pm.MoveInterrupt(true);
            ac.SetBool("holdAtk", false);
            holding = false;
            hit = hitBoxes[0];
            hit.enabled = true;
            readyToAtk = true;
            ac.SetTrigger("holdEnd");
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && readyToAtk)
        {
            Debug.Log("Combo Attack : " + (comboIndex + 1) + " / 3");
            lastPressedTime = Time.time;
            if (firstAtk)
            {
                secondAtk = Time.time - lastPressedTime <= comboTime;
                if (secondAtk)
                {
                    bool thirdAtk = Time.time - lastPressedTime <= comboTime;
                    if(comboIndex == 2 && thirdAtk)
                    {
                        Debug.Log("Combo Finished");
                        firstAtk = false;
                        secondAtk = false;
                        
                    }
                }

            }
            else
            {
                firstAtk = true;
            }
            pm.MoveInterrupt(false); // halts movement for attack
            Attack();
            readyToAtk = false;
        }
        if((firstAtk || secondAtk) && Time.time - lastPressedTime > comboTime)
        {
            Debug.Log("Combo Failed");
            secondAtk = false;
            firstAtk = false;
            comboIndex = 0;
        }
    }

    void Attack()
    {

        ac.SetTrigger("atk");
        ac.SetFloat("atkIndex", comboIndex);
        pm.transform.rotation = pm.orientation.rotation;

        //StartCoroutine(ComboTimer());
        StartCoroutine(EnableHit());
        //    AlternateIndex();

        //    if (crRunning == true)
        //    {
        //        StopAllCoroutines();
        //    }

        //    StartCoroutine(comboTimer());

    }

    //IEnumerator ComboTimer()
    //{
    //    comboWait = true;
    //    while (timer < comboTime)
    //    {
    //        yield return new WaitForSeconds(.1f);
    //        timer += .1f;
    //        if (Input.GetKeyDown(KeyCode.Mouse0)/* && readyToAtk*/)
    //        {
    //            if (comboIndex == 2)
    //            {
    //                comboIndex = 0;
    //                timer = 0;
    //                comboWait = false;
    //                break;
    //            }
    //            else
    //            {
    //                comboIndex += 1;
    //                timer = 0;
    //                comboWait = false;
    //                break;
    //            }
    //        }
    //    }
    //    comboIndex = 0;
    //    timer = 0;
    //    comboWait = false;
    //}

    IEnumerator SpinAttackMov()
    {
        while (holding)
        {
            yield return new WaitForSeconds(.25f);
            rb.AddForce(Random.onUnitSphere * 10f * pm.walkSpeed, ForceMode.Force);
        }
    }

    IEnumerator EnableHit()
    {
        yield return new WaitForSeconds(.6f); // wait enough time for attack to start
        hit.enabled = true;

        yield return new WaitForSeconds(.3f); // turn hitbox on for near end of attack duration
        hit.enabled = false;

        //Collider[] colliders = Physics.OverlapSphere(this.transform.position, 5.0f); // finds all colliders within a radius
        //foreach (Collider hit in colliders)
        //{
        //    Rigidbody rb = hit.GetComponent<Rigidbody>();

        //    if (rb != null)
        //        rb.AddExplosionForce(pc.knockBackModifier, this.transform.position, 5.0f, 3.0f); // adds force to object rigidbody
        //}


        yield return new WaitForSeconds(.3f); // end of attack

        if(comboIndex == 2)
        {
            comboIndex = 0;
        }
        else if (!secondAtk || comboIndex == 1)
        {
            comboIndex += 1;
        }
        //lastPressedTime = Time.time;
        pm.MoveInterrupt(true); // re-enables movement
        readyToAtk = true;

    }

    //IEnumerator comboTimer()
    //{
    //    crRunning = true;

    //    yield return new WaitForSeconds(.5f);

    //    crRunning = false;
    //    ac.SetBool("atk", false);
    //    ac.SetFloat("atkIndex", 0);
    //}

    //private void AlternateIndex()
    //{
    //    if(ac.GetFloat("atkIndex") == 0)
    //    {
    //        ac.SetFloat("atkIndex", 1);
    //    }
    //    else
    //    {
    //        ac.SetFloat("atkIndex", 0);
    //    }
    //}
}
