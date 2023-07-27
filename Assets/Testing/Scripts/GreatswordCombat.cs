using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatswordCombat : MonoBehaviour
{
    public Transform weapon;
    public Transform hitbox;
    Collider hit;
    Animator ac;
    PlayerMovement pm;

    bool crRunning;

    // Start is called before the first frame update
    void Start()
    {
        hit = hitbox.GetComponent<Collider>();
        ac = weapon.GetComponent<Animator>();
        pm = this.GetComponent<PlayerMovement>();

        hit.enabled = false;
        hit.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && CanAttack())
        {
            Attack();
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
        yield return new WaitForSeconds(.05f);
        hit.enabled = true;
        yield return new WaitForSeconds(.3f);
        hit.enabled = false;
        yield return new WaitForSeconds(.05f);

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

    private bool CanAttack()
    {
        if(pm.state == PlayerMovement.MovementState.idle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
