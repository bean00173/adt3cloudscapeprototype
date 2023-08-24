using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRotation : MonoBehaviour
{
    Quaternion rot;
    float yCoord;
    // Start is called before the first frame update
    void Start()
    {
        rot = transform.rotation;
        yCoord = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = rot;
        this.transform.position = new Vector3(this.transform.position.x, yCoord, this.transform.position.z);
    }
}
