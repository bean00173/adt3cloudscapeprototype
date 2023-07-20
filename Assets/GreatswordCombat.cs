using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatswordCombat : MonoBehaviour
{
    public Transform weapon;
    BoxCollider bc;

    // Start is called before the first frame update
    void Start()
    {
        bc = weapon.GetComponent<BoxCollider>();
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

    }
}
