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
        foreach(Transform child in transform) // makes sure all instantiated body parts are in the list
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
                if (child.GetComponent<DespawnTimer>().readyToDespawn) // if child is ready to despawn (alive longer than despawn timer)
                {
                    Destroy(child.gameObject); // destroy
                    bodyParts.RemoveAt(bodyParts.IndexOf(child.gameObject)); // remove from list
                }

                if(bodyParts.Count < 50) // if there's less than 50, dont destroy any more.
                {
                    break;
                }
            }

        }
    }

    public GameObject RandomLimb() // function to grab random limb from possible limb drops
    {
        int randomLimbIndex = Random.Range(0, limbs.Length);
        return limbs[randomLimbIndex];
    }
}
