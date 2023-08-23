using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class AirshipInteraction : MonoBehaviour
{

    public GameObject player;
    Camera mainCamera;

    public Transform airshipObject;
    AirshipMovement am;
    Rigidbody rb;
    bool promptReady;
    bool readyToDock;
    bool docked;
    bool dockingComplete;
    bool aligned;
    bool goingToDock;
    float leastDist;

    Transform closest;
    Transform current;
    Transform currentTower;

    string targetName;

    RaycastHit hit;
    private bool docking;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        am = GetComponent<AirshipMovement>();
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTower = GameObject.Find("Tower").transform;

        if (docked && aligned)
        {
            if (!dockingComplete)
            {
                dockingComplete = true;

                Debug.Log("Docked");
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                //GetComponentInChildren<PlayableCharacter>().CanMove();
                am.enabled = false;
                airshipObject.GetComponent<Animator>().SetBool("moving", false);

                if (GameObject.FindObjectOfType<PlayableCharacter>() == null)
                {
                    GameObject character = Instantiate(player, current.GetChild(1).transform.position, Quaternion.identity);
                    StartCoroutine(DeactivateCombat(character));
                }
            }
        }

        if (docking && !aligned)
        {
            Vector3 delta = new Vector3(current.GetChild(0).position.x - this.transform.position.x, 0f, current.GetChild(0).position.z - this.transform.position.z);  // calculate x/z position difference between agent and player
            Quaternion target = Quaternion.LookRotation(delta); // create new target location based off of x/z diff

            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5.0f); // slerp rotation based on time multiplied by a constant speed

            float diff = transform.rotation.eulerAngles.y - target.eulerAngles.y;
            float degree = 5;
            if (Mathf.Abs(diff) <= degree)
            {
                Debug.Log("Aligned");
                aligned = true;
            }
        }

        if(readyToDock && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Dont want to enter");
            rb.AddForce(-Vector3.left * 100000f);
            am.DisableMovement(false);

            readyToDock = false;
            promptReady = false;
        }

        if(readyToDock && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Entering Island");
            Transform container = currentTower.GetComponent<TowerData>().navPointContainer;
            currentTower.GetComponent<TowerData>().AirshipHitbox.enabled = false;

            StartCoroutine(DoDockingProcedure(container));

            //while (current.gameObject.name != "homePoint" && !docking)
            //{
            //    if (current.gameObject.name == "Point")
            //    {
            //        targetName = "Point (2)";
            //    }
            //    else if (current.gameObject.name == "Point (1)" || current.gameObject.name == "Point (2)")
            //    {
            //        targetName = "homePoint";
            //    }

            //    foreach (Transform child in container)
            //    {
            //        if (child.name == targetName)
            //        {
            //            ContinueDocking(child);
            //            docking = true;
            //        }
            //    }
            //}

            docked = true;


            readyToDock = false;
            promptReady = false;
        }

        if (promptReady)
        {
            GameManager.instance.ReturnUIComponent("PromptParent").GetChild(1).gameObject.SetActive(true);
            readyToDock = true;
        }
        else
        {
            GameManager.instance.ReturnUIComponent("PromptParent").GetChild(1).gameObject.SetActive(false);
        }

        //if (Physics.SphereCast(this.transform.position, 3.0f, transform.forward, out hit, 3.0f) && !promptReady)
        //{
        //    if (hit.collider.CompareTag("Tower"))
        //    {
        //        am.DisableMovement(true);
        //        promptReady = true;
        //    }
        //    else
        //    {
        //        promptReady = false;
        //    }
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            am.DisableMovement(true);
            promptReady = true;
        }
        else
        {
            promptReady = false;
        }
    }

    private Transform NavToPoint(Transform container)
    {
        foreach(Transform child in container)
        {
            float dist = Vector3.Distance(this.transform.position, child.transform.position);
            Debug.Log(child.name + " " + dist);
            if (leastDist == 0)
            {
                leastDist = dist;
                closest = child;
            }
            else if(dist < leastDist)
            {
                leastDist = dist;
                closest = child;
            }
            else if(dist == leastDist && child.gameObject.name == "homePoint")
            {
                closest = child;
            }
        }

        return closest;
    }

    private void CurrentDockPos(Transform child)
    {
        current = child;
    }

    private IEnumerator DoDockingProcedure(Transform container)
    {
        startTime = Time.time;

        docking = true;
        float duration = 3.0f;
        float time = 0;

        Transform child = NavToPoint(container);
        Vector3 endPos = child.position; // using function to receive target transform from closest child gameobject inside container
        Vector3 startPos = this.transform.position;

        CurrentDockPos(child);

        while (time < duration)
        {
            this.transform.position = Vector3.Lerp(startPos, endPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        docking = false;
    }

    private IEnumerator DeactivateCombat(GameObject character)
    {
        //bool characterLoaded = false;
        Character.CharacterId characterId;

        //while (!characterLoaded)
        //{
        //    foreach(Transform child in character.GetComponentInChildren<PlayableCharacter>().characterContainer)
        //    {
        //        if(child.gameObject.activeSelf)
        //        {
        //            characterLoaded = true; break;
        //        }
        //    }
        //}

        yield return new WaitUntil(() => character.GetComponentInChildren<PlayableCharacter>().charLoaded);

        characterId = character.GetComponentInChildren<PlayableCharacter>().currentCharacter;

        // WHEN MORE CHARACTERS AVAILABLE ADD SWITCH STATEMENT HERE

        character.GetComponentInChildren<PlayableCharacter>().ReturnCurrentCharacter().GetComponent<GreatswordCombat>().enabled = false; // Change this when all characters are playable
    }

    //private IEnumerator ContinueDocking(Transform next)
    //{
    //    docking = true;
    //    startTime = Time.time;
    //    float duration = 1.0f;
    //    float time = 0;

    //    Vector3 target = next.position; // using function to receive target transform from closest child gameobject inside container
    //    Vector3 startPos = this.transform.position;

    //    CurrentDockPos(next);
    //    docking = false;

    //    while (time < duration)
    //    {
    //        this.transform.position = Vector3.Lerp(startPos, target, time / duration);
    //        time += Time.deltaTime;
    //        yield return null;
    //    }


    //}
}
