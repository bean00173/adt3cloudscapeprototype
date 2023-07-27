using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Limb
{
    public GameObject prefab;
    public int probability;
    public bool selected;
}

public class BodyPartContainer : MonoBehaviour
{

    public List<Limb> limbs = new List<Limb> ();
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

    public void DropLimbs(int count, Vector3 spawnPos) // function to grab random limb from possible limb drops
    {
        Debug.Log(count);
        for(int i = 0; i <= count - 1; i++)
        {
            foreach (Limb limb in limbs)
            {
                if (GameManager.instance.RandomChance(limb.probability) && !limb.selected)
                {
                    Debug.Log(limb.prefab + "CHOSEN!!!");
                    Instantiate(limb.prefab, SpawnPosition(i, count, spawnPos), Quaternion.identity, this.transform);
                    limb.selected = true;
                }
                else
                {
                    Debug.Log(limb.prefab + "DENIED!!");
                }
            }
        }
        ResetSpawns();
    }

    private void ResetSpawns()
    {
        foreach (Limb limb in limbs)
        {
            limb.selected = false;
        }
    }

    private Vector3 SpawnPosition(int i, int x, Vector3 pos)
    {
        float rad = 2 * Mathf.PI / x * i + Random.Range(-1f, 1f); // divides radius by how many objects are instantiated and spaces them semi evenly (with a little sprinkle of randomisation)
        float vert = Mathf.Sin(rad); // calculates x,z coordinates based on angle from origin
        float hor = Mathf.Cos(rad);

        Vector3 spawnDir = new Vector3(hor, 0, vert); // creates vector with coords

        return pos + spawnDir * .5f; // return spawnPos
    }
}
