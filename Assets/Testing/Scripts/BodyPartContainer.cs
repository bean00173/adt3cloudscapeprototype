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

    public List<Limb> gruntLimbs = new List<Limb> ();
    public List<Limb> bruteLimbs = new List<Limb>();
    public List<Limb> rangerLimbs = new List<Limb>();

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

        if(bodyParts.Count >= 50) //if there are too many bodyParts in the scene, need to remove oldest ones
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<DespawnTimer>().readyToDespawn) // if child is ready to despawn (alive longer than despawn timer, rather than bubble sort --> save effort)
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

    public void DropLimbs(int count, Vector3 spawnPos, enemyType type) // function to grab random limb from possible limb drops
    {
        List<Limb> limbs = new List<Limb>();
        switch (type) // switch the list of limbs to be used based on received enemy type from function call
        {
            case enemyType.grunt:
                limbs = new List<Limb>(gruntLimbs); // this used to be limbs = gruntLimbs but that only stores a reference, not list data, so when the list gets cleared it was clearing gruntLimbs too, throwing up an exception
                break;
            case enemyType.brute:
                limbs = new List<Limb>(bruteLimbs);
                break;
            case enemyType.ranger:
                limbs = new List<Limb>(rangerLimbs);
                break;
            default:
                break;
        }
        Debug.Log(count);
        for(int i = 0; i <= count - 1; i++) // loop through amount of times based on how many limbs are to be spawned
        {
            foreach (Limb limb in limbs) // check each limb in list
            {
                if (GameManager.instance.RandomChance(limb.probability) && !limb.selected) // if RandomChance returns true based on limb probability, and that limb has not been selected previously
                {
                    Debug.Log(limb.prefab + "CHOSEN!!!");
                    Instantiate(limb.prefab, SpawnPosition(i, count, spawnPos), Quaternion.identity, this.transform); // instantiate limb 
                    limb.selected = true; // selected = true so that limb cannot be spawned multiple times (two left legs wouldn't make sense)
                    break;
                }
            }
            limbs[limbs.Count - 1].selected = false; //  grab last limb (always manually set to be 100 probability) and make not selected so that if no other limb is selected, it will always default to this one (will be just a generic body chunk)
            /*
            NOTE : It is going to be better eventually to figure out how to spawn a default limb 
            rather than have to follow a set of rules when creating the list itself
            */
        }
        ResetSpawns(limbs);
    }

    private void ResetSpawns(List<Limb> limbs) // once all limbs have been spawned, make sure each selected is false so next of same type can still drop
    {
        foreach (Limb limb in limbs)
        {
            limb.selected = false;
        }
        limbs.Clear(); // remove all elements from generic list in case next enemy is different type
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
