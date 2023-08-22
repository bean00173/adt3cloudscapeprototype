using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{

    public bool inAirship = true;
    Transform currentTower;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentTower = GameObject.Find("Tower").transform;

        Vector3 aimTarget = currentTower.position;
        Vector3 target = new Vector3(aimTarget.x - this.transform.position.x, 0f, aimTarget.z - this.transform.position.z);
        Quaternion aimDir = Quaternion.LookRotation(target);
        transform.rotation = Quaternion.Slerp(transform.rotation, aimDir, Time.deltaTime * 1.0f);
    }
}
