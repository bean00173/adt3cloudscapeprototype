using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class GreatswordCombat : MonoBehaviour
{
    public Transform hitContainer;
    Transform player;
    List<Collider> colliders = new List<Collider>();
    Collider hit;
    PlayableCharacter pc;
    Animator ac;
    PlayerMovement pm;
    Rigidbody rb;

    bool readyToAtk = true;
    bool readyToHold = true;
    bool holding;

    public float holdCd;

    float lastPressedTime;
    int comboIndex;
    public float comboTime = .25f;
    bool firstAtk;
    bool secondAtk;

    public bool abilityReady;

    public GameObject abilityText,/* abilityIcon,*/ abilityProgress, abilityPS, shakeManager;

    // Start is called before the first frame update
    void Start()
    {
        player = this.transform.root.GetChild(0);
        foreach(Collider col in hitContainer.GetComponentsInChildren<Collider>())
        {
            colliders.Add(col);
            col.enabled = false; // disables collider and enables as trigger
            col.isTrigger = true;
        }
        ac = this.GetComponentInChildren<Animator>();
        pm = player.GetComponent<PlayerMovement>();
        rb = player.GetComponent<Rigidbody>();
        pc = player.GetComponent<PlayableCharacter>();

        abilityReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        hit = colliders[comboIndex];
        //if (Input.GetKey(KeyCode.Mouse0))
        //{
        //    Debug.Log("HOLDING ATTACK");
        //}
        //if (Input.GetKey(KeyCode.F) && readyToHold) // if holding down F key
        //{
        //    //StartCoroutine(SpinAttackMov());
        //    pm.MoveInterrupt(false); // pause movement
        //    holding = true;
        //    readyToAtk = false; 
        //    hit.enabled = true;
        //    hit = colliders[0]; // set hitbox to be sphere collider (hard coded)
        //    ac.SetBool("holdAtk", true); // begin spinning attack animation
        //}
        //else if(holding == true) // if holding is true but key not pressed
        //{
        //    this.transform.rotation = pm.orientation.transform.rotation; // set rotation of player to be orientation rotation
        //    pm.MoveInterrupt(true); // allow movement
        //    ac.SetBool("holdAtk", false); // disable animation
        //    holding = false;
        //    readyToHold = true;
        //    hit = colliders[0];
        //    hit.enabled = false;
        //    readyToAtk = true;
        //    ac.SetTrigger("holdEnd"); // begin ending animation
        //}

        if (Input.GetKeyDown(KeyCode.X) && abilityReady)
        {
            StartCoroutine(Ability());
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && readyToAtk) // if mouse clicked
        {
            Debug.Log("Combo Attack : " + (comboIndex + 1) + " / 3");
            lastPressedTime = Time.time; // set recent click time 
            if (firstAtk) // if one attack already done
            {
                secondAtk = Time.time - lastPressedTime <= comboTime; // check time between now and last pressed, true if less than combo timer
                if (secondAtk)
                {
                    bool thirdAtk = Time.time - lastPressedTime <= comboTime; // same as before but for third attack
                    if(comboIndex == 2 && thirdAtk) // if combo ready for last hit 
                    {

                        ac.SetTrigger("atkEnd");
                        firstAtk = false; // reset bools
                        secondAtk = false;
                        
                    }
                }

            }
            else
            {
                firstAtk = true; // first attack
            }
            pm.MoveInterrupt(false); // halts movement for attack
            Attack(); // call attack
            readyToAtk = false; // disable interrupting attacks
        }
        if((firstAtk || secondAtk) && Time.time - lastPressedTime > comboTime) // regardless of mouse click, if first or second click achieved + time has expired, fail combo, reset combo progress
        {
            Debug.Log("Combo Failed");
            ac.SetTrigger("atkEnd");
            secondAtk = false;
            firstAtk = false;
            comboIndex = 0;
        }
    }
    void Attack()
    {

        ac.Play("Attack", -1, 0f); // if attack called, begin animations, set state of combo blend tree
        ac.SetFloat("atkIndex", comboIndex);
        readyToHold = false;
        pm.transform.rotation = pm.orientation.rotation; // set rotation of player to orientation rotation
        //StartCoroutine(EnableHit()); // run coroutine for enabling hitbox

    }

    IEnumerator SpinAttackMov()
    {
        while (holding)
        {
            yield return new WaitForSeconds(.25f);
            rb.AddForce(Random.onUnitSphere * 10f * pm.walkMultiplier * pc.speedModifier, ForceMode.Force);
        }
    }

    public void hitOn()
    {
        hit.enabled = true;

        StartCoroutine(GameManager.instance.DoCameraShake(shakeManager.GetComponentInChildren<CinemachineFreeLook>(), .25f));
    }

    public void hitOff()
    {
        hit.enabled = false;

        if (comboIndex == 2) // if combo index already maxed out, reset index
        {
            comboIndex = 0;
            Debug.Log("Combo Finished");
        }
        else if (!secondAtk || comboIndex == 1) // if combo index not 2 or only on first attack, increase index by one
        {
            comboIndex += 1;
        }
        pm.MoveInterrupt(true); // re-enables movement
        readyToAtk = true; // re-enables attack capability
        readyToHold = true; // re enable special atk capability
    }

    IEnumerator Ability()
    {
        DoAbility();

        Instantiate(abilityPS, transform.position, Quaternion.identity, transform);

        //Image image = abilityIcon.GetComponent<Image>();
        //image.color = new Color(image.color.r, image.color.g, image.color.b, .5f);
        abilityReady = false;
        yield return StartCoroutine(Timer(pc.abilityCd));
        abilityReady = true;
    }

    private void DoAbility()
    {
        BodyPartContainer bpc = GameObject.Find("BodyParts").GetComponent<BodyPartContainer>();
        foreach(Transform child in bpc.gameObject.transform)
        {
            if (child.gameObject.tag == "HealthOrb")
            {
                StartCoroutine(OrbMagnet(child.gameObject));
            }
        }


    }
    
    private IEnumerator OrbMagnet(GameObject go)
    {
        float duration = 1;

        float time = 0;
        Vector3 startPosition = go.transform.position;
        while (time < duration)
        {
            if(go == null)
            {
                time = duration;
                break;
            }
            go.transform.position = Vector3.Lerp(startPosition, this.transform.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        if(go != null)
        {
            go.transform.position = this.transform.position;
        }
        else
        {
            yield return null;
        }
        
    }

    IEnumerator Timer(float time)
    {
        float currentTime = 0;
        abilityText.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(time - currentTime) + "s";
        while (currentTime < time)
        {
            yield return new WaitForSeconds(0.1f);
            abilityProgress.GetComponent<Image>().fillAmount = 1 - (currentTime / time);
            abilityText.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(time - currentTime) + "s";
            currentTime += .1f;
        }
        //Image image = abilityIcon.GetComponent<Image>();
        //image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        abilityProgress.GetComponent<Image>().fillAmount = 0;
        abilityText.GetComponent<TextMeshProUGUI>().text = "";

    }

    //IEnumerator EnableHit()
    //{
    //    yield return new WaitForSeconds(.6f); // wait enough time for attack to start
    //    hit.enabled = true;

    //    yield return new WaitForSeconds(.3f); // turn hitbox on for near end of attack duration
    //    hit.enabled = false;

    //    //Collider[] colliders = Physics.OverlapSphere(this.transform.position, 5.0f); // finds all colliders within a radius
    //    //foreach (Collider hit in colliders)
    //    //{
    //    //    Rigidbody rb = hit.GetComponent<Rigidbody>();

    //    //    if (rb != null)
    //    //        rb.AddExplosionForce(pc.knockBackModifier, this.transform.position, 5.0f, 3.0f); // adds force to object rigidbody
    //    //}


    //    yield return new WaitForSeconds(.3f); // end of attack

        

    //}
}
