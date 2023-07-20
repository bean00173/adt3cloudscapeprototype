using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatswordCombat : MonoBehaviour
{
    public Transform weapon;
    BoxCollider bc;
    Animator ac;

    // Start is called before the first frame update
    void Start()
    {
        bc = weapon.GetComponent<BoxCollider>();
        ac = weapon.GetComponent<Animator>();
        bc.enabled = false;
        bc.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (this.ac.GetCurrentAnimatorStateInfo(0).IsName("Swing") || this.ac.GetCurrentAnimatorStateInfo(0).IsName("Swing 2"))
        {
            ac.SetTrigger("Combo");
        }
        else
        {
            ac.SetTrigger("Attack");
        }
    }
}
