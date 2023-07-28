using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    public GameObject bloodSplat;
    private Transform parent;
    RaycastHit hit;

    bool splattered;

    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.Find("BloodContainer").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.red, .1f);
        if(!splattered && Physics.Raycast(transform.position, Vector3.down, out hit, .5f, LayerMask.GetMask("Ground"))) // if the current limb hasn't yet splattered blood, raycast towards the ground
        {
            splattered = true; // if ground near, find point of contact, create vector pos and instantiate splatter
            Vector3 pos = new Vector3((float)hit.point.x, (float)hit.point.y + 0.1f, (float)hit.point.z);
            Instantiate(bloodSplat, pos, Quaternion.identity, parent);

        }
    }

    
}
