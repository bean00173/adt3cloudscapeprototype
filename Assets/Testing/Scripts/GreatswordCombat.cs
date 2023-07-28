using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatswordCombat : MonoBehaviour
{
    public Transform weapon;
    public Transform hitbox;
    Collider hit;
    PlayableCharacter pc;
    Animator ac;
    PlayerMovement pm;
    Rigidbody rb;

    bool readyToAtk = true;
    bool crRunning;

    // Start is called before the first frame update
    void Start()
    {
        hit = hitbox.GetComponent<Collider>();
        ac = weapon.GetComponent<Animator>();
        pm = this.GetComponent<PlayerMovement>();
        rb = this.GetComponent<Rigidbody>();
        pc = this.GetComponent<PlayableCharacter>();

        hit.enabled = false;
        hit.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && readyToAtk)
        {
            pm.MoveInterrupt(false); // halts movement for attack
            Attack();
            readyToAtk = false;
        }
    }

    void Attack()
    {

        ac.SetTrigger("atk");
        pm.transform.rotation = pm.orientation.rotation;

        StartCoroutine(EnableHit());
        //    AlternateIndex();

        //    if (crRunning == true)
        //    {
        //        StopAllCoroutines();
        //    }

        //    StartCoroutine(comboTimer());

    }

    IEnumerator EnableHit()
    {
        yield return new WaitForSeconds(.1f); // wait enough time for attack to start
        hit.enabled = true;

        yield return new WaitForSeconds(.8f); // turn hitbox on for most of attack duration
        hit.enabled = false;


        //Collider[] colliders = Physics.OverlapSphere(this.transform.position, 5.0f); // finds all colliders within a radius
        //foreach (Collider hit in colliders)
        //{
        //    Rigidbody rb = hit.GetComponent<Rigidbody>();

        //    if (rb != null)
        //        rb.AddExplosionForce(pc.knockBackModifier, this.transform.position, 5.0f, 3.0f); // adds force to object rigidbody
        //}


        yield return new WaitForSeconds(.3f); // end of attack
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
