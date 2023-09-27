using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OnStep : MonoBehaviour
{
    public float stepShakeForce = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StepShake()
    {
        this.GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(stepShakeForce);
    }
}
