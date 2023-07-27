using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartContainer : MonoBehaviour
{

    public GameObject[] limbs;
    public List<GameObject> bodyParts = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Transform child in transform)
        {
            if (!bodyParts.Contains(child.gameObject))
            {
                bodyParts.Add(child.gameObject);
            }
        }

        if(bodyParts.Count >= 50)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<DespawnTimer>().readyToDespawn)
                {
                    Destroy(child.gameObject);
                    bodyParts.RemoveAt(bodyParts.IndexOf(child.gameObject));
                }

                if(bodyParts.Count < 50)
                {
                    break;
                }
            }

        }
    }

    public GameObject RandomLimb()
    {
        int randomLimbIndex = Random.Range(0, limbs.Length);
        return limbs[randomLimbIndex];
    }
}
