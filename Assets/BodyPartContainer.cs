using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartContainer : MonoBehaviour
{

    public GameObject[] limbs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.childCount < 50f)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<DespawnTimer>().time <= 2.0f)
                {
                    Destroy(child.gameObject);
                }
            }

        }
    }
}
