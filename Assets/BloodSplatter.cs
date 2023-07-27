using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    public GameObject bloodSplat;
    RaycastHit hit;

    bool splattered;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!splattered && Physics.Raycast(transform.position, Vector3.down, out hit, 1.0f, LayerMask.GetMask("Ground")))
        {
            splattered = true;
            Vector3 pos = new Vector3((float)hit.point.x, (float)hit.point.y + 0.1f, (float)hit.point.z);
            Instantiate(bloodSplat, pos, Quaternion.identity);

        }
    }

    
}
