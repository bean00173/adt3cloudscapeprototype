using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Cinemachine;

public class AirshipInteraction : MonoBehaviour
{

    public GameObject player;
    public GameObject character { get; private set; }
    Camera mainCamera;

    public Transform airshipObject;
    public GameObject dockPrompt;
    public GameObject playerUi;
    public float dockingDistance = 200f;
    AirshipMovement am;
    Rigidbody rb;
    bool promptReady; //
    bool readyToDock; //
    bool docked; //
    bool dockingComplete; //
    bool aligned; //
    bool goingToDock; //
    float leastDist; //

    Transform lookTarget; //

    Transform closest; //
    Transform current; //
    Transform currentTower; //

    string targetName;

    RaycastHit hit;
    private bool docking;
    float startTime;

    float dist;

    AudioSource source;
    public UnityEvent dock = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        am = GetComponent<AirshipMovement>();
        rb = GetComponentInChildren<Rigidbody>();
        source = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        currentTower = TowerManager.instance.currentTower.transform;

        Debug.DrawLine(this.transform.position, currentTower.position, Color.yellow);
        //Debug.Log(Vector3.Distance(this.transform.position, currentTower.position));

        if (Vector3.Distance(this.transform.position, currentTower.position) < dockingDistance && !(docking || docked))
        {
            promptReady = true;
        }
        else
        {
            promptReady = false;
        }

        if (current != null)
        {
            dist = Vector3.Distance(this.transform.position, current.position);
            Debug.DrawLine(this.transform.position, current.position, Color.yellow);
            //Debug.Log(dist);

            if (dist < 5.0f)
            {
                docked = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        if (docked && aligned)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            am.emitting = false;
            am.cloudsPS.GetComponent<ParticleSystem>().Stop();

            if (!dockingComplete)
            {
                dockingComplete = true;

                Debug.Log("Docked");

                dockPrompt.transform.parent.gameObject.SetActive(false);

                current.GetChild(2).gameObject.SetActive(true);

                //GetComponentInChildren<PlayableCharacter>().CanMove();

                if (GameObject.FindObjectOfType<PlayableCharacter>() == null)
                {
                    Spawn();
                    playerUi.SetActive(true);
                }
            }
        }

        if ((docking || docked) && !aligned)
        {
            Vector3 delta = new Vector3(current.GetChild(0).position.x - this.transform.position.x, 0f, current.GetChild(0).position.z - this.transform.position.z);  // calculate x/z position difference between agent and player
            Quaternion target = Quaternion.LookRotation(delta); // create new target location based off of x/z diff

            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 10.0f); // slerp rotation based on time multiplied by a constant speed

            float diff = transform.rotation.eulerAngles.y - target.eulerAngles.y;
            float degree = 5;
            if (Mathf.Abs(diff) <= degree && docked)
            {
                Debug.Log("Aligned");
                aligned = true;
            }
        }

        //if(readyToDock && Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Debug.Log("Dont want to enter");
        //    rb.AddForce(-Vector3.left * 100000f);
        //    am.DisableMovement(false);

        //    readyToDock = false;
        //    promptReady = false;
        //}

        if(readyToDock && Input.GetKeyDown(KeyCode.E))
        {
            dock?.Invoke();
            source.clip = AudioManager.instance.GetClip(AudioType.effects, "Airship_Bell");
            source.Play();
            Debug.Log("Entering Island");
            Transform container = currentTower.GetComponent<TowerData>().navPointContainer;
            //currentTower.GetComponent<TowerData>().AirshipHitbox.enabled = false;
            airshipObject.GetComponent<Animator>().SetBool("moving", false);

            GameManager.instance.towerLeft = false;


            am.canMove = false;
            am.enabled = false;

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
            


            readyToDock = false;
            promptReady = false;
        }

        if (promptReady)
        {
            dockPrompt.SetActive(true);
            readyToDock = true;
        }
        else
        {
            dockPrompt.SetActive(false);
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

    public void RedoSpawn()
    {
        am.ac.Play("Startup");
        character = null;
        playerUi.SetActive(true);
        Invoke(nameof(Spawn), 2.0f);
    }

    private void Spawn()
    {
        //lookTarget.gameObject.SetActive(false);
        character = Instantiate(player, current.GetChild(1).transform.position, current.GetChild(0).transform.rotation * Quaternion.Euler(0, 180, 0));
        character.GetComponentInChildren<PlayableCharacter>().fell.AddListener(RedoSpawn);
        foreach(Transform ci in playerUi.transform)
        {
            if (ci.GetComponent<CharacterInterface>())
            {
                character.GetComponentInChildren<PlayableCharacter>().worldCollide.AddListener(ci.GetComponent<CharacterInterface>().Fell);
                ci.GetComponent<CharacterInterface>().character = character.GetComponentInChildren<PlayableCharacter>();
            }
            
        }
        StartCoroutine(DeactivateCombat(character));
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Tower"))
    //    {
    //        am.DisableMovement(true);
    //        if (!promptReady)
    //        {
    //            promptReady = true;
    //        }
    //    }
    //    else
    //    {
    //        promptReady = false;
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Tower"))
    //    {
    //        am.DisableMovement(true);
    //        if (!promptReady)
    //        {
    //            promptReady = true;
    //        }
    //    }
    //    else
    //    {
    //        promptReady = false;
    //    }
    //}

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
        }

        return closest;
    }

    private void CurrentDockPos(Transform child)
    {
        current = child;
    }

    private IEnumerator DoDockingProcedure(Transform container)
    {
        this.GetComponent<Collider>().enabled = false;

        startTime = Time.time;

        docking = true;

        
        float time = 0;

        Transform child = NavToPoint(container);
        Vector3 endPos = child.position; // using function to receive target transform from closest child gameobject inside container
        Vector3 startPos = this.transform.position;

        CurrentDockPos(child);

        float duration = 3.0f + (3.0f * ( 1 - (1 / Mathf.Sqrt(Vector3.Distance(this.transform.position, endPos) / 100))));

        while (time < duration)
        {
            
            this.transform.position = Vector3.Lerp(startPos, endPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(Timer(3f));

        yield return new WaitUntil(() => Vector3.Distance(this.transform.position, endPos) < 5.0f);

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

        character.GetComponentInChildren<PlayableCharacter>().ReturnCurrentCharacter().GetComponentInChildren<Combat>().enabled = false; // Change this when all characters are playable
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

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);

        if (!dockingComplete)
        {
            StopAllCoroutines();
            am.enabled = true;
            ResetDockStatus();
        }
    }
    public void ResetDockStatus()
    {
        playerUi.SetActive(false);
        am.canMove = true;
        promptReady = false;
        readyToDock = false;
        docking = false;
        docked = false;
        dockingComplete = false;
        aligned = false;
        current = null;
        leastDist = 0;
        dist = 0;
        closest = null;
    }
}
