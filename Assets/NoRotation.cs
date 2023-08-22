using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRotation : MonoBehaviour
{
    Quaternion rot;
    // Start is called before the first frame update
    void Start()
    {
       rot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = rot;
    }
}
