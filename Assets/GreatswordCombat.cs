using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatswordCombat : MonoBehaviour
{
    public Transform weapon;
    Collider bc;
    Animator ac;

    bool crRunning;

    // Start is called before the first frame update
    void Start()
    {
        bc = weapon.GetComponentInChildren<Collider>();
        ac = weapon.GetComponent<Animator>();
        bc.enabled = false;
        bc.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)/* && CanAttack()*/)
        {
            Attack();
        }
    }

    void Attack()
    {
        ac.SetTrigger("atk");
        //    AlternateIndex();

        //    if (crRunning == true)
        //    {
        //        StopAllCoroutines();
        //    }

        //    StartCoroutine(comboTimer());

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

    //private bool CanAttack()
    //{
    //    return !ac.GetBool("atk");
    //}
}
