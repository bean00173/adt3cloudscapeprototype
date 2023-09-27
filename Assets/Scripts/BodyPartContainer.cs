using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Drop
{
    public GameObject prefab;
    public int probability;
    public bool selected;
}
[System.Serializable]
public class Ragdoll
{
    public GameObject prefab;
    public Character.CharacterId correspondingChar;
    public int probability;
}

public class BodyPartContainer : MonoBehaviour
{

    //public List<Drop> gruntLimbs = new List<Drop> ();
    //public List<Drop> bruteLimbs = new List<Drop>();
    //public List<Drop> rangerLimbs = new List<Drop>();

    public Drop healthOrb = new Drop();

    List<GameObject> bodyParts = new List<GameObject>();

    public List<Ragdoll> bruteDolls = new List<Ragdoll>();
    public List<Ragdoll> gruntDolls = new List<Ragdoll>();
    public List<Ragdoll> rangerDolls = new List<Ragdoll>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //foreach(Transform child in transform) // makes sure all instantiated body parts are in the list
        //{
        //    if (!bodyParts.Contains(child.gameObject))
        //    {
        //        bodyParts.Add(child.gameObject);
        //    }
        //}

        //if(bodyParts.Count >= 200) //if there are too many bodyParts in the scene, need to remove oldest ones
        //{
        //    foreach (Transform child in transform)
        //    {
        //        if (child.GetComponent<DespawnTimer>().readyToDespawn) // if child is ready to despawn (alive longer than despawn timer, rather than bubble sort --> save effort)
        //        {
        //            Destroy(child.gameObject); // destroy
        //            bodyParts.RemoveAt(bodyParts.IndexOf(child.gameObject)); // remove from list
        //        }

        //        if(bodyParts.Count < 50) // if there's less than 50, dont destroy any more.
        //        {
        //            break;
        //        }
        //    }

        //}
    }

    public void SpawnRagdoll(Vector3 spawnPos, enemyType type)
    {
        List<Ragdoll> dolls = new List<Ragdoll>();
        switch (type)
        {
            case enemyType.grunt:
                dolls = new List<Ragdoll>(gruntDolls); // this used to be limbs = gruntLimbs but that only stores a reference, not list data, so when the list gets cleared it was clearing gruntLimbs too, throwing up an exception
                break;
            case enemyType.brute:
                dolls = new List<Ragdoll>(bruteDolls);  // [CHANGE] temporarily use placeholder limbs for all others [CHANGE WHEN MORE ENEMY MODEL IMPLEMENTATION]
                break;
            case enemyType.ranger:
                dolls = new List<Ragdoll>(rangerDolls);
                break;
            default:
                break;
        }

        bool spawned = false;
        while (!spawned)
        {
            foreach (Ragdoll rd in dolls)
            {
                if (rd.correspondingChar == GameManager.activeCharacter.Id)
                {
                    if (GameManager.instance.RandomChance(rd.probability) && !spawned)
                    {
                        Debug.Log($"Current character is {GameManager.activeCharacter.Id}, so {rd.prefab.name} was chosen!");
                        Instantiate(rd.prefab, spawnPos, Quaternion.identity, this.transform);
                        break;
                    }
                }
            }
        }

    }

    //public void DropLimbs(int count, Vector3 spawnPos, enemyType type) // function to grab random limb from possible limb drops
    //{
    //    List<Drop> limbs = new List<Drop>();
    //    switch (type) // switch the list of limbs to be used based on received enemy type from function call
    //    {
    //        case enemyType.grunt:
    //            limbs = new List<Drop>(gruntLimbs); // this used to be limbs = gruntLimbs but that only stores a reference, not list data, so when the list gets cleared it was clearing gruntLimbs too, throwing up an exception
    //            break;
    //        case enemyType.brute:
    //            limbs = new List<Drop>(bruteLimbs);  // [CHANGE] temporarily use placeholder limbs for all others [CHANGE WHEN MORE ENEMY MODEL IMPLEMENTATION]
    //            break;
    //        case enemyType.ranger:
    //            limbs = new List<Drop>(rangerLimbs);
    //            break;
    //        default:
    //            break;
    //    }
    //    Debug.Log(count);
    //    for(int i = 0; i <= count - 1; i++) // loop through amount of times based on how many limbs are to be spawned
    //    {
    //        foreach (Drop limb in limbs) // check each limb in list
    //        {
    //            if (GameManager.instance.RandomChance(limb.probability) && !limb.selected) // if RandomChance returns true based on limb probability, and that limb has not been selected previously
    //            {
    //                Debug.Log(limb.prefab + "CHOSEN!!!");
    //                Instantiate(limb.prefab, GameManager.instance.SpawnPosition(i, count, spawnPos, .5f), Quaternion.identity, this.transform); // instantiate limb 
    //                limb.selected = true; // selected = true so that limb cannot be spawned multiple times (two left legs wouldn't make sense)
    //                break;
    //            }
    //        }
    //        limbs[limbs.Count - 1].selected = false; //  grab last limb (always manually set to be 100 probability) and make not selected so that if no other limb is selected, it will always default to this one (will be just a generic body chunk)
    //        /*
    //        NOTE : It is going to be better eventually to figure out how to spawn a default limb 
    //        rather than have to follow a set of rules when creating the list itself
    //        */
    //    }
    //    ResetSpawns(limbs);
    //}

    //private void ResetSpawns(List<Drop> limbs) // once all limbs have been spawned, make sure each selected is false so next of same type can still drop
    //{
    //    foreach (Drop limb in limbs)
    //    {
    //        limb.selected = false;
    //    }
    //    limbs.Clear(); // remove all elements from generic list in case next enemy is different type
    //}

    public void HealthDrop(Vector3 spawnPos)
    {
        if (GameManager.instance.RandomChance(healthOrb.probability)) Instantiate(healthOrb.prefab, spawnPos, Quaternion.identity, this.transform);
    }
}
